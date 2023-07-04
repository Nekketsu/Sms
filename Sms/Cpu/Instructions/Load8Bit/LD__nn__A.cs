namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__nn__A : Instruction
    {
        public override uint Cycles => 13;
        public override byte[] OpCodes { get; } = { 0b00110010 };

        public LD__nn__A(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            Z80.Memory[nn] = Z80.Registers.A;
        }

        public override string ToString(byte opCode)
        {
            var nn = Z80.Memory.ReadWord((ushort)(Z80.Registers.PC + 1));

            return $"ld (0x{nn:x}), a";
        }
    }
}
