namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD__nn__dd : EdInstruction
    {
        public override uint Cycles => 20;
        public override byte[] OpCodes { get; }

        public LD__nn__dd(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b1000011;

            OpCodes = Z80.Alu.Registers16Bit.Indices.Select(r => (byte)(opCodeBase | (r << 4))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var dd = (opCode & 0b00110000) >> 4;
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            Z80.Memory.WriteWord(nn, Z80.Alu.Registers16Bit[dd]);
        }
    }
}
