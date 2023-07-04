namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class CCF : Instruction
    {
        public override uint Cycles => 4;

        public override byte[] OpCodes { get; } = { 0b00111111 };

        public CCF(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, Z80.Registers.F.HasFlag(Registers.Flags.C));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, !Z80.Registers.F.HasFlag(Registers.Flags.C));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
        }

        public override string ToString(byte opCode)
        {
            return "ccf";
        }
    }
}
