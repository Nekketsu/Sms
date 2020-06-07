namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_A__BC_ : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b00001010 };

        public LD_A__BC_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.A = Z80.Memory[Z80.Registers.BC];
        }
    }
}
