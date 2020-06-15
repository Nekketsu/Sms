namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class XOR__HL_ : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b10101110 };
        public XOR__HL_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory[Z80.Registers.HL];

            Z80.Alu.Xor(value);
        }
    }
}
