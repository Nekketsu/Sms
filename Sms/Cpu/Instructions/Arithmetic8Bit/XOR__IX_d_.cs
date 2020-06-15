namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class XOR__IX_d_ : DdInstruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; } = { 0b10101110 };
        public XOR__IX_d_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = Z80.Memory[Z80.Registers.PC++];
            var value = Z80.Memory[(ushort)(Z80.Registers.IX + d)];

            Z80.Alu.Xor(value);
        }
    }
}
