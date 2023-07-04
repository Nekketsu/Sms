namespace Sms.Cpu.Instructions.Load16Bit
{
    public class POP_qq : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; }

        public POP_qq(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000001;

            OpCodes = Z80.Alu.Registers16Bit.Indices.Select(r => (byte)(opCodeBase | (r << 4))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var qq = (opCode & 0b00110000) >> 4;

            if (qq == 0b11)
            {
                Z80.Registers.AF = Z80.Memory.ReadWord(Z80.Registers.SP);
            }
            else
            {
                Z80.Alu.Registers16Bit[qq] = Z80.Memory.ReadWord(Z80.Registers.SP);
            }
            Z80.Registers.SP += 2;
        }

        public override string ToString(byte opCode)
        {
            var qq = (opCode & 0b00110000) >> 4;

            var register = qq == 0b11 ? "af" : Z80.Alu.Registers16Bit.Names[qq];
            var value = (qq == 0b11 ? Z80.Registers.AF : Z80.Alu.Registers16Bit[qq]);

            Console.WriteLine($"pop 0x{value:x}");

            return $"pop {register}";
        }
    }
}
