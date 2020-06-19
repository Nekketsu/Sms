namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class CPL : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00101111 };

        public CPL(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.A = (byte)~Z80.Registers.A;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, true);
        }
    }
}
