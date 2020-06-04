namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_A_I : EdInstruction
    {
        public override uint Cycles => 2;
        public override byte[] OpCodes { get; } = { 0b01010111 };

        public LD_A_I(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.A = Z80.Registers.I;
        }
    }
}
