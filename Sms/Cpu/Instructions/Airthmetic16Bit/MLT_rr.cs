namespace Sms.Cpu.Instructions.Airthmetic16Bit
{
    public class MLT_rr : EdInstruction
    {
        public override uint Cycles => 17;
        public override byte[] OpCodes { get; }

        public MLT_rr(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01001100;

            OpCodes = Z80.Alu.Registers16Bit.Indices.Select(r => (byte)(opCodeBase | (r << 4))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = (opCode & 0b00110000) >> 4;

            var value = Z80.Alu.Registers16Bit[r];
            var h = (byte)(value >> 8);
            var l = (byte)(value & 0x00ff);

            Z80.Alu.Registers16Bit[r] = Z80.Mlt(h, l);
        }

        public override string ToString(byte opCode)
        {
            var r = (opCode & 0b00110000) >> 4;

            return $"mlt {Z80.Alu.Registers16Bit.Names[r]}";
        }
    }
}
