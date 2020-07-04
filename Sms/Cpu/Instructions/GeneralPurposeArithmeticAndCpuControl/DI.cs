namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class DI : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b11110011 };

        public DI(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.IFF1 = false;
            Z80.Registers.IFF2 = false;
        }
    }
}
