namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class ADC_A__HL_ : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b10001110 };
        public ADC_A__HL_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory[Z80.Registers.HL];
            var CY = Z80.Registers.F.HasFlag(Registers.Flags.C) ? 1 : 0;

            Z80.Alu.Add(value, CY);
        }
    }
}
