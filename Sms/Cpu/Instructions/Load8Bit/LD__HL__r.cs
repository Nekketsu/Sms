using System.Linq;

namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__HL__r : Instruction
    {
        public override uint Cycles => 2;
        public override byte[] OpCodes { get; }

        const byte OpCodeBase = 0b01110000;

        public LD__HL__r(Z80 z80) : base(z80)
        {
            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(OpCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var source = (opCode & ~OpCodeBase) << 3;

            Z80.Memory[Z80.Registers.HL] = Z80.Alu.Registers8Bit[source];
        }
    }
}
