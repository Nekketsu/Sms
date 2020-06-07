namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__BC__A : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b00000010 };

        public LD__BC__A(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Memory[Z80.Registers.BC] = Z80.Registers.A;
        }
    }
}
