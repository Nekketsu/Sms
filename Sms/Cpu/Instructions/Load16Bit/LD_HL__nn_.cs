namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD_HL__nn_ : Instruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b00101010 };

        public LD_HL__nn_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            Z80.Registers.HL = Z80.Memory.ReadWord(nn);
        }

        public override string ToString(byte opCode)
        {
            var nn = Z80.Memory.ReadWord((ushort)(Z80.Registers.PC + 1));

            return $"ld hl, (0x{nn:x})";
        }
    }
}
