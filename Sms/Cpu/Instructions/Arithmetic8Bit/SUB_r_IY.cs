namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class SUB_r_IY : FdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; }
        public SUB_r_IY(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b10010000;

            OpCodes = Z80.Alu.Registers8BitIY.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;

            Z80.Alu.Sub(Z80.Alu.Registers8BitIY[r]);
        }

        public override string ToString(byte opCode)
        {
            var r = opCode & 0b00000111;

            var register = Z80.Alu.Registers8BitIY.Names[r];

            return $"sub {register}";
        }
    }
}
