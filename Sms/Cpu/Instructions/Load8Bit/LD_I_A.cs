namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_I_A : EdInstruction
    {
        public override uint Cycles => 2;
        public override byte[] OpCodes { get; } = { 0b01000111 };

        public LD_I_A(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.I = Z80.Registers.A;
        }
    }
}
