namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class ADC_A_n : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b11001110 };
        public ADC_A_n(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];
            var CY = Z80.Registers.F.HasFlag(Registers.Flags.C) ? 1 : 0;

            Z80.Alu.Add(n, CY);
        }
    }
}
