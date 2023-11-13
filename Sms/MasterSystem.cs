using Sms.Memory;
using System.Diagnostics;

namespace Sms
{
    public class MasterSystem
    {
        const double ClicksPerSecond = 10738580;

        public const ushort PageLength = 0x4000;
        const ushort romLength = 3 * PageLength;

        public Z80 Z80 { get; }
        public TMS9918A Vdp { get; set; }
        public Cartridge Cartridge { get; set; }


        public MasterSystem()
        {
            Z80 = new Z80();
        }

        public void MainLoop()
        {
            const double Sixthyth = 1000 / 60; // We're working in milliseconds
            double lastFrameTime = 0;
            /*while (notQuitting) // Loop until the player quits
            {
                var currentTime = GetCurrentTime();

                // Draw a frame every 1/60 of a second
                if (lastFrameTime + Sixthyth <= currentTime)
                {
                    lastFrameTime = currentTime;
                    Update(); // This will draw one frame
                }
            }*/
        }

        // This is responsible for emulating one frame
        public void Update()
        {
            const double MachineClickPerFrame = ClicksPerSecond / 60;
            var clicksThisUpdate = 0u;

            // Emulate 1/60th of a seconds amount of machine clicks
            while (clicksThisUpdate < MachineClickPerFrame)
            {
                try
                {
                    var z80ClockCycles = Z80.ExecuteNextInstruction();

                    HandleInterrupts();
                    // The machine clock is 3 times faster than the z80 clock
                    var machineClicks = z80ClockCycles * 3u;

                    // The VDP clock is half the speed of the machine clock
                    var vdpCyles = (float)machineClicks / 2;

                    // The sound clock is the same speed as the Z80
                    var soundCycles = z80ClockCycles;

                    Vdp.Update(vdpCyles);
                    //Sound.Update(soundCycles);

                    clicksThisUpdate += machineClicks;
                }
                catch (NotImplementedException e)
                {
                    Debug.WriteLine($"The OP Code {e.Message} is not implemented");
                }
            }
        }

        private void HandleInterrupts()
        {
            if (Z80.Registers.NMI && !Z80.State.NMIServicing)
            {
                Z80.State.NMIServicing = true;
                Z80.Registers.NMI = false;
                Z80.Registers.IFF1 = false;
                Z80.State.Halted = false;
                Z80.Alu.PushWordOnStack(Z80.Registers.PC);
                Z80.Registers.PC = 0x66;
            }

            if (Vdp.IsRequestingInterrupt)
            {
                if (Z80.Registers.IFF1 && Z80.State.InterruptMode == 1)
                {
                    Z80.State.Halted = false;
                    Z80.Alu.PushWordOnStack(Z80.Registers.PC);
                    Z80.Registers.PC = 0x38;
                    Z80.Registers.IFF1 = false;
                    Z80.Registers.IFF2 = false;
                }
            }
        }
    }
}
