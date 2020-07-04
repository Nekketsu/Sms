namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class EI : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b11111011 };

        public EI(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.IFF1 = true;
            Z80.Registers.IFF2 = true;
        }
    }
}
