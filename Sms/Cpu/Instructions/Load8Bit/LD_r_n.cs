using System.Linq;

namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_r_n : Instruction
    {
        public override uint Cycles => 2;
        public override byte[] OpCodes { get; }

        const byte OpCodeBase = 0b00000110;

        public LD_r_n(Z80 z80) : base(z80)
        {
            OpCodes = z80.Alu.Registers8Bit.Indices.Select(r => (byte)(OpCodeBase | (r << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var destination = (opCode & ~OpCodeBase) >> 3;
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Alu.Registers8Bit[destination] = n;
        }
    }
}
