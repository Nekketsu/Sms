namespace Sms.Cpu.Instructions.Jump
{
    public class JP__HL_ : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b11101001 };

        public JP__HL_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.PC = Z80.Registers.HL;
        }

        public override string ToString(byte opCode)
        {
            return "jp (hl)";
        }
    }
}
