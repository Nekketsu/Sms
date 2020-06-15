namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class INC__IY_d_ : FdInstruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; } = { 0b00110100 };
        public INC__IY_d_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = Z80.Memory[Z80.Registers.PC++];
            var value = Z80.Memory[(ushort)(Z80.Registers.IY + d)];

            Z80.Memory[(ushort)(Z80.Registers.IY + d)] = Z80.Alu.Inc(value);
        }
    }
}
