namespace Sms.Cpu.Instructions.BitSetResetAndTest
{
    public class SET_b__IX_d_ : DdCbInstruction
    {
        public override uint Cycles => 23;
        public override byte[] OpCodes { get; }

        public SET_b__IX_d_(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000110;
            var bValues = Enumerable.Range(0, 8);

            OpCodes = bValues.Select(b => (byte)(opCodeBase | (b << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var b = (opCode & 0b00111000) >> 3;
            var d = (sbyte)Z80.Memory[(ushort)(Z80.Registers.PC - 2)];

            var value = Z80.Memory[(ushort)(Z80.Registers.IX + d)];
            value = value.SetBit(b);
            Z80.Memory[(ushort)(Z80.Registers.IX + d)] = value;
        }
    }
}
