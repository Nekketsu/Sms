namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD__nn__HL : Instruction
    {
        public override uint Cycles => 5;
        public override byte[] OpCodes { get; } = { 0b00100010 };

        public LD__nn__HL(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            Z80.Memory.WriteWord(nn, Z80.Registers.HL);
        }
    }
}
