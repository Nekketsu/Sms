using System.Linq;

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

            Z80.Alu.Registers16Bit[qq] = Z80.Memory.ReadWord(Z80.Registers.SP);
            Z80.Registers.SP += 2;
        }
    }
}
