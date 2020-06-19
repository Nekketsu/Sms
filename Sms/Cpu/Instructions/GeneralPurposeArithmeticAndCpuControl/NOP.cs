namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class NOP : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00000000 };

        public NOP(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            // The CPU performs no operation during this machine cycle.
        }
    }
}
