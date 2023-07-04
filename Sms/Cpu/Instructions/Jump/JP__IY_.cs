namespace Sms.Cpu.Instructions.Jump
{
    public class JP__IY_ : FdInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; } = { 0b11101001 };

        public JP__IY_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.PC = Z80.Memory[Z80.Registers.IY];
        }

        public override string ToString(byte opCode)
        {
            return "jp (iy)";
        }
    }
}
