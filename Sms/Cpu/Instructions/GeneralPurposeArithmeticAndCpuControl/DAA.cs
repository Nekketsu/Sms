using Sms.Cpu.Instructions.Arithmetic8Bit;
using static Sms.Cpu.Registers;

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
            var cf = Z80.Registers.F.HasFlag(Flags.C);
            var highNibble = (byte)((Z80.Registers.A & 0xF0) >> 4);
            var hf = Z80.Registers.F.HasFlag(Flags.H);
            var lowNibble = (byte)(Z80.Registers.A & 0x0F);

            var adjustment = (cf, highNibble, hf, lowNibble) switch
            {
                (false, >= 0x0 and <= 0x9, false, >= 0x0 and <= 0x9) => 0x0,
                (false, >= 0x0 and <= 0x9, true,  >= 0x0 and <= 0x9) => 0x6,
                (false, >= 0x0 and <= 0x8, _,     >= 0xA and <= 0xF) => 0x6,
                (false, >= 0xA and <= 0xF, false, >= 0x0 and <= 0x9) => 0x60,
                (true,  _,                 false, >= 0x0 and <= 0x9) => 0x60,
                (true,  _,                 true,  >= 0x0 and <= 0x9) => 0x66,
                (true,  _,                 _,     >= 0xA and <= 0xF) => 0x66,
                (false, >= 0x9 and <= 0xF, _,     >= 0xA and <= 0xF) => 0x66,
                (false, >= 0xA and <= 0xF, true,  >= 0x0 and <= 0x9) => 0x66,
                _ => throw new NotImplementedException()
            };

            var cfAfter = (cf, highNibble, lowNibble) switch
            {
                (false, >= 0x0 and <= 0x9, >= 0x0 and <= 0x9) => false,
                (false, >= 0x0 and <= 0x8, >= 0xA and <= 0xF) => false,
                (false, >= 0x9 and <= 0xF, >= 0xA and <= 0xF) => true,
                (false, >= 0xA and <= 0xF, >= 0x0 and <= 0x9) => true,
                (true,  _,                 _                ) => true,
                _ => throw new NotImplementedException()
            };

            var nf = Z80.Registers.F.HasFlag(Flags.N);
            var hfAfter = (nf, hf, lowNibble) switch
            {
                (false, _,     >= 0x0 and <= 0x9) => false,
                (false, _,     >= 0xA and <= 0xF) => true,
                (true,  false, _                ) => false,
                (true,  true,  >= 0x6 and <= 0xf) => false,
                (true,  true,  >= 0x0 and <= 0x5) => true,
                _ => throw new NotImplementedException()
            };

            
            if (Z80.Registers.F.HasFlag(Flags.N))
            {
                Z80.Registers.A -= (byte)adjustment;
            }
            else
            {
                Z80.Registers.A += (byte)adjustment;
            }

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, hfAfter);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, Z80.Registers.A.HasEvenParity());
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, cfAfter);
        }

        public override string ToString(byte opCode)
        {
            return "daa";
        }
    }
}
