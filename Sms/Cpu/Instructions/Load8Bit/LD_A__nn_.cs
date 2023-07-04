namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_A__nn_ : Instruction
    {
        public override uint Cycles => 13;
        public override byte[] OpCodes { get; } = { 0b00111010 };

        public LD_A__nn_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            Z80.Registers.A = Z80.Memory[nn];
        }

        public override string ToString(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);

            return $"ld a, (0x{nn:x})";
        }
    }
}
