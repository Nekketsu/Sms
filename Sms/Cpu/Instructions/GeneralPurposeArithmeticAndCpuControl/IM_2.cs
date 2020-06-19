namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class IM_2 : EdInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; } = { 0b01011110 };

        public IM_2(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.State.InterruptMode = 2;
        }
    }
}
