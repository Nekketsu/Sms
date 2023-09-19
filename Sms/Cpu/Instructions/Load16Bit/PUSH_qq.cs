namespace Sms.Cpu.Instructions.Load16Bit
{
    public class PUSH_qq : Instruction
    {
        public override uint Cycles => 11;
        public override byte[] OpCodes { get; }

        public PUSH_qq(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000101;

            OpCodes = Z80.Alu.Registers16Bit.Indices.Select(r => (byte)(opCodeBase | (r << 4))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var qq = (opCode & 0b00110000) >> 4;

            Z80.Registers.SP -= 2;
            if (qq == 0b11)
            {
                Z80.Memory.WriteWord(Z80.Registers.SP, Z80.Registers.AF);
            }
            else
            {
                Z80.Memory.WriteWord(Z80.Registers.SP, Z80.Alu.Registers16Bit[qq]);
            }
        }

        public override string ToString(byte opCode)
        {
            var qq = (opCode & 0b00110000) >> 4;
            var register = qq == 0b11 ? "af" : Z80.Alu.Registers16Bit.Names[qq];

            return $"push {register}";
        }
    }
}
