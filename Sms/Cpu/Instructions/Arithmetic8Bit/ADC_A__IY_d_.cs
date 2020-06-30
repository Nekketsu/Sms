namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class ADC_A__IY_d_ : FdInstruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; } = { 0b10001110 };
        public ADC_A__IY_d_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = Z80.Memory[Z80.Registers.PC++];
            var value = Z80.Memory[(ushort)(Z80.Registers.IY + d)];
            var CY = Z80.Registers.F.HasFlag(Registers.Flags.C);

            Z80.Alu.Add(value, CY);
        }
    }
}
