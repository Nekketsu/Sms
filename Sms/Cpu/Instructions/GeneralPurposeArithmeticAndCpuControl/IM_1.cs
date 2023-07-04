namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class IM_1 : EdInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; } = { 0b1010110 };

        public IM_1(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.State.InterruptMode = 1;
        }

        public override string ToString(byte opCode)
        {
            return "im 1";
        }
    }
}
