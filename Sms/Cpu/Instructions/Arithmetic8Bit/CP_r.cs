namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class CP_r : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; }
        public CP_r(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b10111000;

            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;
            var value = Z80.Alu.Registers8Bit[r];

            Z80.Alu.Compare(value);
        }
    }
}
