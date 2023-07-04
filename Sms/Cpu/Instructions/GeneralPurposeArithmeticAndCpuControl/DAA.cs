namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class DAA : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00100111 };

        public DAA(Z80 z80) : base(z80)
        {
        }

        protected override void InnerExecute(byte opCode)
        {
            var cf = Z80.Registers.F.HasFlag(Registers.Flags.C);
            var highNibble = (byte)((Z80.Registers.A & 0xF0) >> 4);
            var hf = Z80.Registers.F.HasFlag(Registers.Flags.H);
            var lowNibble = (byte)(Z80.Registers.A & 0x0F);

            var diff = 0;
            if (cf)
            {
                if (lowNibble >= 0xA && lowNibble <= 0xF)
                {
                    diff = 66;
                }
                else if (lowNibble >= 0 && lowNibble <= 9)
                {
                    diff = hf ? 66 : 60;
                }
            }
            else
            {
                if (lowNibble >= 0xA && lowNibble <= 0xF)
                {
                    if (highNibble >= 0 && highNibble <= 8)
                    {
                        diff = 6;
                    }
                    else if (highNibble >= 9 && highNibble <= 0xF)
                    {
                        diff = 66;
                    }
                }
                else if (lowNibble >= 0 && lowNibble <= 9)
                {
                    if (highNibble >= 0 && highNibble <= 9)
                    {
                        diff = hf ? 6 : 0;
                    }
                    else if (highNibble >= 0xA && highNibble <= 0xF)
                    {
                        diff = hf ? 66 : 60;
                    }
                }

                Z80.Registers.A += (byte)(Z80.Registers.F.HasFlag(Registers.Flags.N) ? -diff : diff);

                if (!cf)
                {
                    if (lowNibble >= 0 && lowNibble <= 9)
                    {
                        if (highNibble >= 0 && highNibble <= 9)
                        {
                            cf = false;
                        }
                        else if (highNibble >= 0xA && highNibble <= 0xF)
                        {
                            cf = true;
                        }
                    }
                    else if (lowNibble >= 0xA && lowNibble <= 0xF)
                    {
                        if (highNibble >= 0 && highNibble <= 8)
                        {
                            cf = false;
                        }
                        else if (highNibble >= 9 && highNibble <= 0xF)
                        {
                            cf = true;
                        }
                    }
                }
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cf);

                if (!Z80.Registers.F.HasFlag(Registers.Flags.N))
                {
                    if (lowNibble >= 0 && lowNibble <= 9)
                    {
                        hf = false;
                    }
                    else if (lowNibble >= 0xA && lowNibble <= 0xF)
                    {
                        hf = true;
                    }
                }
                else
                {
                    if (!hf)
                    {
                        hf = false;
                    }
                    else if (hf)
                    {
                        if (lowNibble >= 6 && lowNibble <= 0xF)
                        {
                            hf = false;
                        }
                        else if (lowNibble >= 0 && lowNibble <= 5)
                        {
                            hf = true;
                        }
                    }
                }

                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, hf);

                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, Z80.Registers.A.HasBit(7));
                //Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Y, Z80.Registers.A.HasBit(5));
                //Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.X, Z80.Registers.A.HasBit(3));

                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, Z80.Registers.A == 0);
            }
        }

        public override string ToString(byte opCode)
        {
            return "daa";
        }
    }
}
