namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_R_A : EdInstruction
    {
        public override uint Cycles => 2;
        public override byte[] OpCodes { get; } = { 0b01001111 };

        public LD_R_A(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.R = Z80.Registers.A;
        }
    }
}
