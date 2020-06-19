namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class SC : Instruction
    {
        public override uint Cycles => 4;

        public override byte[] OpCodes { get; } = { 0b00110111 };

        public SC(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, true);
        }
    }
}
