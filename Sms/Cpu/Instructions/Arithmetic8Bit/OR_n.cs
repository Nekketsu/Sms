namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class OR_n : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b11110110 };
        public OR_n(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Alu.Or(n);
        }

        public override string ToString(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            return $"or 0x{n:x}";
        }
    }
}
