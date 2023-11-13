namespace Sms.Cpu.Instructions.Jump
{
    public class JP__IX_ : DdInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; } = { 0b11101001 };

        public JP__IX_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.PC = Z80.Registers.IX;
        }

        public override string ToString(byte opCode)
        {
            return "jp (ix)";
        }
    }
}
