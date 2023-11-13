namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class SBC_A__IX_d_ : DdInstruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; } = { 0b10011110 };
        public SBC_A__IX_d_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = (sbyte)Z80.Memory[Z80.Registers.PC++];
            var value = Z80.Memory[(ushort)(Z80.Registers.IX + d)];
            var CY = Z80.Registers.F.HasFlag(Registers.Flags.C);

            Z80.Alu.Sub(value, CY);
        }
    }
}
