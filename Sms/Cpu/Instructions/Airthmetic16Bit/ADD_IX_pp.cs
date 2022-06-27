namespace Sms.Cpu.Instructions.Airthmetic16Bit
{
    public class ADD_IX_pp : DdInstruction
    {
        public override uint Cycles => 15;

        public override byte[] OpCodes { get; }

        public ADD_IX_pp(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b00001001;

            OpCodes = Z80.Alu.Registers16Bit.Indices.Select(r => (byte)(opCodeBase | (r << 4))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = (opCode & 0b00110000) >> 4;
            var value = Z80.Alu.Registers16Bit[r];

            Z80.Registers.IX = Z80.Alu.Add(Z80.Registers.IX, value);
        }
    }
}
