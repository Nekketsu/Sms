using System.Linq;

namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__IY_d__r : FdInstruction
    {
        public override uint Cycles => 5;
        public override byte[] OpCodes { get; }

        const byte OpCodeBase = 0b01110000;

        public LD__IY_d__r(Z80 z80) : base(z80)
        {
            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(OpCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var source = (opCode & ~OpCodeBase) << 3;
            var d = Z80.Memory[Z80.Registers.PC++];

            Z80.Memory[(ushort)(Z80.Registers.IY + d)] = Z80.Alu.Registers8Bit[source];
        }
    }
}
