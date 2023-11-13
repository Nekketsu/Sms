namespace Sms.Cpu.Instructions.BitSetResetAndTest
{
    public class BIT_b__HL_ : CbInstruction
    {
        public override uint Cycles => 12;
        public override byte[] OpCodes { get; }

        public BIT_b__HL_(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01000110;
            var bValues = Enumerable.Range(0, 8);

            OpCodes = bValues.Select(b => (byte)(opCodeBase | (b << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var b = (opCode & 0b00111000) >> 3;

            var value = Z80.Memory[Z80.Registers.HL];

            Z80.Alu.TestBit(value, b, opCode == 0x7e);
        }
    }
}
