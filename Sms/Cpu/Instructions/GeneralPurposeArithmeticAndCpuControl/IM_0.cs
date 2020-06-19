namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class IM_0 : EdInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; } = { 0b01000110 };

        public IM_0(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.State.InterruptMode = 0;
        }
    }
}
