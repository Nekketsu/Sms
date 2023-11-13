namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class AND__IY_d_ : FdInstruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; } = { 0b10100110 };
        public AND__IY_d_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = (sbyte)Z80.Memory[Z80.Registers.PC++];
            var value = Z80.Memory[(ushort)(Z80.Registers.IY + d)];

            Z80.Alu.And(value);
        }
    }
}
