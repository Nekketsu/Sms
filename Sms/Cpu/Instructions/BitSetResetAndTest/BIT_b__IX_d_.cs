namespace Sms.Cpu.Instructions.BitSetResetAndTest
{
    public class BIT_b__IX_d_ : DdCbInstruction
    {
        public override uint Cycles => 20;
        public override byte[] OpCodes { get; }

        public BIT_b__IX_d_(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01000000;
            var bValues = Enumerable.Range(0, 8);

            OpCodes = bValues.SelectMany(b1 => bValues.Select(b2 => (byte)(opCodeBase | b1 << 3 | b2))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var b = (opCode & 0b00111000) >> 3;
            var d = (sbyte)Z80.Memory[(ushort)(Z80.Registers.PC - 2)];

            var value = Z80.Memory[(ushort)(Z80.Registers.IX + d)];

            Z80.Alu.TestBit(value, b, opCode >= 0x78);
        }
    }
}
