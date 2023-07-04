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
            var pp = (opCode & 0b00110000) >> 4;
            
            var value = (pp == 0b10)
                ? Z80.Registers.IX
                : Z80.Alu.Registers16Bit[pp];

            Z80.Registers.IX = Z80.Alu.Add(Z80.Registers.IX, value);
        }

        public override string ToString(byte opCode)
        {
            var pp = (opCode & 0b00110000) >> 4;

            var register = (pp == 0b10)
                ? "ix"
                : Z80.Alu.Registers16Bit.Names[pp];

            return $"add ix, {register}";
        }
    }
}
