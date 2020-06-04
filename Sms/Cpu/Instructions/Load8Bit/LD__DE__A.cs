namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__DE__A : Instruction
    {
        public override uint Cycles => 2;
        public override byte[] OpCodes { get; } = { 0b00010010 };

        public LD__DE__A(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Memory[Z80.Registers.DE] = Z80.Registers.A;
        }
    }
}
