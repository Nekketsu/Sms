using Sms.Vdp;
using System;
using System.Collections.Generic;

namespace Sms
{
    public class TMS9918A : IPortMapping
    {
        public Dictionary<byte, Func<byte>> PortReaders => new Dictionary<byte, Func<byte>>
        {
            [0x7E] = () => VCounter,
            [0x7F] = () => HCounter,
            [0xBE] = () => DataPort,
            [0xBF] = () => ControlPort
        };

        public Dictionary<byte, Action<byte>> PortWriters => new Dictionary<byte, Action<byte>>
        {
            [0xBE] = data => DataPort = data,
            [0xBF] = data => ControlPort = data
        };

        /// <summary>
        /// Video RAM
        /// </summary>
        public Ram VRam { get; }
        /// <summary>
        /// Color RAM
        /// </summary>
        public Ram CRam { get; }

        public Registers Registers { get; set; }

        public Screen Screen { get; private set; }

        Dictionary<int, IVdpRenderer> renderers;


        public const int NumResHorizontal = 256;
        public const int NumResVertical = 192;
        public const int NumResVertMed = 224;
        public const int NumResVertHigh = 240;

        bool isPal;


        bool isSecondControlWrite;
        byte readBuffer;
        public bool IsRequestingInterrupt { get; private set; }

        public byte HCounter { get; private set; }
        public byte VCounter { get; private set; }
        bool vCounterFirst;

        bool isVBlank;
        bool refresh;

        float runningCycles;

        byte lineInterrupt;

        public bool ShiftX => Registers[0].HasBit(3);
        public bool LineInterruptEnabled => Registers[0].HasBit(4);
        // Will we be setting the first column to the backdrop color?
        public bool MaskFirstColumn => Registers[0].HasBit(5);

        // The following will stop horizontal scrolling and vertical scrolling for the top column and left column of display
        public bool LimitHScroll => Registers[0].HasBit(6);
        public bool LimitVScroll => Registers[0].HasBit(7);

        public bool IsZommed => Registers[0x1].HasBit(0);
        public bool Is8x16 => Registers[0x1].HasBit(1);
        public bool FrameInterruptEnabled => Registers[1].HasBit(5);
        public bool DisplayVisible => Registers[1].HasBit(6);
        public bool DisplayBlanked => !Registers[1].HasBit(6);

        public ushort ColorTableBase => (ushort)(Registers[3].HasBit(7) ? 0x2000 : 0x0);
        public byte ColAnd => ((byte)((Registers[3] & 127) << 1)).SetBit(0);

        public ushort PatternTableBase => (ushort)(Registers[4].HasBit(2) ? 0x2000: 0x0);
        public int PgTable
        {
            get
            {
                var row = VCounter / 8;

                // The pattern table is comprised of 3 table (0-2), which one are we using
                var pgTable = 0;

                // Are we drawing part of the lower 2 thirds of the screen?
                if (row > 7)
                {
                    // if we are drawing bottom third
                    if (row > 15)
                    {
                        // Then use table 3 if bit 1 of register 4 is set else use table 0
                        if (Registers[4].HasBit(1))
                        {
                            pgTable = 2;
                        }
                        // We must be drawing the middle third
                        else
                        {
                            // Then use table 2 if bit 0 of register 4 is set else use table 0
                            if (Registers[4].HasBit(0))
                            {
                                pgTable = 1;
                            }
                        }
                    }
                }

                return pgTable;
            }
        }

        public ushort PgOffset
        {
            get
            {
                // pgOffset points us to the correct pg table to use (this is also the same for the color table)
                var pgOffset = (ushort)0;

                if (PgTable == 1)
                {
                    pgOffset = 256 * 8;
                }
                else if (PgTable == 2)
                {
                    pgOffset = 256 * 2 * 8;
                }

                return pgOffset;
            }
        }

        public ushort SpriteAttributeTableBase => (ushort)((Registers[5] & 0x7E) << 7);

        public bool UseSecondPattern => Registers[6].HasBit(2);
        public ushort SgTable => (ushort)((Registers[6] & 7) << 11);
        public byte HScroll => Registers[0x8];
        public byte VScroll => Registers[0x9];
        public byte LineCounter => Registers[0xA];

        private byte Mode
        {
            get
            {
                var res = (byte)0;

                res |= (byte)((Registers[0].HasBit(2) ? 1 : 0) << 3);
                res |= (byte)((Registers[1].HasBit(3) ? 1 : 0) << 2);
                res |= (byte)((Registers[0].HasBit(1) ? 1 : 0) << 1);
                res |= (byte)(Registers[1].HasBit(4) ? 1 : 0);

                return res;
            }
        }


        ushort commandWord;
        private byte CodeRegister => (byte)(commandWord >> 14);
        private ushort AddressRegister => (ushort)(commandWord & 0x3FFF);

        public TMS9918A()
        {
            VRam = new Ram(0x4000);
            CRam = new Ram(32);
            Registers = new Registers();

            renderers = new Dictionary<int, IVdpRenderer>
            {
                [4] = new Mode4Renderer(this)
            };
        }

        private void IncrementAddress()
        {
            // Wrap address register at 0x3FFF
            if (AddressRegister == 0x3FFF)
            {
                commandWord &= 0xC000; // Keep control word code unchanged
            }
            else
            {
                commandWord++;
            }
        }

        /// <summary>
        /// Data Port: 0xBE
        /// </summary>
        public byte DataPort
        {
            get
            {
                isSecondControlWrite = false;

                var data = readBuffer;

                readBuffer = VRam[AddressRegister];
                IncrementAddress();

                return data;
            }
            set
            {
                isSecondControlWrite = false;

                switch (CodeRegister)
                {
                    case 0:
                        VRam[AddressRegister] = value;
                        break;
                    case 1:
                        VRam[AddressRegister] = value;
                        break;
                    case 2:
                        VRam[AddressRegister] = value;
                        break;
                    case 3:
                        CRam[(ushort)(AddressRegister & 31)] = value;
                        break;
                }

                readBuffer = value;

                IncrementAddress();
            }
        }

        /// <summary>
        /// Control Port: 0xBF
        /// </summary>
        public byte ControlPort
        {
            get
            {
                var status = Registers.Status;

                if (Mode == 2)
                {
                    Registers.Status &= 0x2F; // Turn off bits 7 and 5
                }
                else
                {
                    Registers.Status &= 0x1F; // Turn off top 3 bits
                }

                isSecondControlWrite = false;
                IsRequestingInterrupt = false;

                return status;
            }
            set
            {
                if (isSecondControlWrite)
                {
                    // Update the top byte
                    commandWord &= 0xFF;
                    commandWord |= (ushort)(value << 8);
                    isSecondControlWrite = false;

                    // Act on the control code
                    switch (CodeRegister)
                    {
                        case 0:
                            readBuffer = VRam[AddressRegister];
                            IncrementAddress();
                            break;
                        case 2:
                            SetRegisterData();
                            break;
                    }
                }
                else
                {
                    // Update lower byte
                    isSecondControlWrite = true;
                    commandWord &= 0xFF00;
                    commandWord |= value;
                }
            }
        }

        private void SetRegisterData()
        {
            // The new register data is the lower byte
            var data = (byte)(commandWord & 0xFF);
            // Register is lower 4 bits of upper byte;
            var register = (byte)((commandWord >> 8) & 0xF);

            if (register > 11)
            {
                return;
            }

            Registers[register] = data;

            // Is this register write enabling vsync interrupts?
            // If so do we have an IRQ pending?
            if (register == 1)
            {
                if (FrameInterruptPending && FrameInterruptEnabled)
                {
                    IsRequestingInterrupt = true;
                }
            }
        }

        public void Update(float nextCycle)
        {
            IsRequestingInterrupt = false;
            var hCount = HCounter;
            var nextLine = false;
            isVBlank = false;
            refresh = false;

            runningCycles += nextCycle;

            // Ignore everything after the decimal point;
            var clockInfo = (int)Math.Floor(runningCycles);

            // The machine cycle is twice the speed of the vdp and this is the
            // speed the hCounter increments at
            var cycles = clockInfo * 2;

            // Are we moving off this scanline onto the next?
            if (hCount + cycles > 684)
            {
                nextLine = true;

                // If we are starting a new scanline reset the hCounter
                HCounter = (byte)((HCounter + cycles) % 685);
            }

            // We are moving onto the next scanline
            if (nextLine)
            {
                // Store current scanline
                var vCount = VCounter;
                VCounter++; // Move onto the next scanline

                // Are we coming to the end of the vertical refresh?
                // If so we are starting a new frame from scanline 0
                if (vCount == 255)
                {
                    VCounter = 0;
                    vCounterFirst = true;
                    Render();
                    refresh = true;
                }
                // Is it time to jump to vCounter backwards?
                else if ((vCount == VJump) && vCounterFirst)
                {
                    vCounterFirst = false;
                    VCounter = VJumpTo;
                }
                // Are we just about to enter vertical refresh?
                else if (VCounter == Screen.Height)
                {
                    isVBlank = true;
                    FrameInterruptPending = true; // IRQ pending
                }

                if (VCounter >= Screen.Height)
                {
                    // Do not reload the line interrupt until we are past the
                    // FIRST line of the none active display period
                    if (VCounter > Screen.Height)
                    {
                        lineInterrupt = LineCounter;
                    }

                    // Are we changing the screen resolution?
                    Screen = Mode switch
                    {
                        11 => new Screen(NumResHorizontal, NumResVertMed),
                        14 => new Screen(NumResHorizontal, NumResVertHigh),
                        _ => new Screen(NumResHorizontal, NumResVertical)
                    };
                }

                // If we are in active display then draw next scanline
                if (VCounter < Screen.Height)
                {
                    Screen.Disabled = DisplayBlanked;
                    Render();
                }

                // Decremeent the line interrupt counter during the active period
                // including the first line of the none active display period
                if (VCounter <= Screen.Height)
                {
                    var underflow = false;
                    if (lineInterrupt == 0)
                    {
                        underflow = true;
                    }
                    lineInterrupt--;

                    // It is going to underflow
                    if (underflow)
                    {
                        lineInterrupt = LineCounter;
                        if (LineInterruptEnabled)
                        {
                            IsRequestingInterrupt = true;
                        }
                    }
                }
            }

            // Do we want to signal an interrupt
            if (FrameInterruptPending && FrameInterruptEnabled)
            {
                IsRequestingInterrupt = true;
            }
        }

        private byte VJump
        {
            get
            {
                return (isPal, Screen.Height) switch
                {
                    (true, NumResVertical) => 0xF2,
                    (true, NumResVertMed) => 0xFF,
                    (true, _) => 0xFF,

                    (false, NumResVertical) => 0xDA,
                    (false, NumResVertMed) => 0xEA,
                    (false, _) => 0xFF
                };
            }
        }

        private byte VJumpTo
        {
            get
            {
                return (isPal, Screen.Height) switch
                {
                    (true, NumResVertical) => 0xBA,
                    (true, NumResVertMed) => 0xC7,
                    (true, _) => 0xC1,

                    (false, NumResVertical) => 0xD5,
                    (false, NumResVertMed) => 0xE5,
                    (false, _) => 0xFF
                };
            }
        }

        public void Render()
        {
            renderers[Mode].Render();
        }


        public ushort NameBase
        {
            get
            {
                var register2 = (ushort)Registers[2];

                if (Screen.Height == NumResVertical) // Using small res
                {
                    // Bit 0 is ignored so is top nibble
                    register2 &= 0xF;
                    register2 = register2.ResetBit(0);
                    register2 <<= 10;
                }
                else // Must be medium or large res
                {
                    register2 &= 0xC;
                    register2 <<= 10;
                    register2 |= 0x700;
                }

                return register2;
            }
        }

        public bool SpriteCollision
        {
            get => Registers.Status.HasBit(5);
            set => Registers.Status = Registers.Status.SetBit(5, value);
        }

        public bool SpriteOverFlow
        {
            get => Registers.Status.HasBit(6);
            set => Registers.Status = Registers.Status.SetBit(6, value);
        }

        public bool FrameInterruptPending
        {
            get => Registers.Status.HasBit(7);
            set => Registers.Status = Registers.Status.SetBit(7, value);
        }
    }
}
