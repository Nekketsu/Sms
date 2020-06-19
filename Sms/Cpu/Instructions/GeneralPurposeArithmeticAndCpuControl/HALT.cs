namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class HALT : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b01110110 };

        public HALT(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.State.Halted = true;
        }
    }
}
