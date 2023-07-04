namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD_dd__nn_ : EdInstruction
    {
        public override uint Cycles => 20;
        public override byte[] OpCodes { get; }

        public LD_dd__nn_(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01001011;

            OpCodes = Z80.Alu.Registers16Bit.Indices.Select(r => (byte)(opCodeBase | (r << 4))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var dd = (opCode & 0b00110000) >> 4;
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            Z80.Alu.Registers16Bit[dd] = Z80.Memory.ReadWord(nn);
        }

        public override string ToString(byte opCode)
        {
            var dd = (opCode & 0b00110000) >> 4;
            var nn = Z80.Memory.ReadWord((ushort)(Z80.Registers.PC + 1));

            var regiser = Z80.Alu.Registers16Bit.Names[dd];

            return $"ld {regiser}, (0x{nn:x})";
        }
    }
}
