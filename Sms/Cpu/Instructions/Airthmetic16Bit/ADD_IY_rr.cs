using System.Linq.Expressions;

namespace Sms.Cpu.Instructions.Airthmetic16Bit
{
    public class ADD_IY_rr : FdInstruction
    {
        public override uint Cycles => 15;

        public override byte[] OpCodes { get; }

        public ADD_IY_rr(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b00001001;

            OpCodes = Z80.Alu.Registers16Bit.Indices.Select(r => (byte)(opCodeBase | (r << 4))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var registerPointers = new Dictionary<int, Func<Registers, ushort>>
            {
                [0b00] = r => r.BC,
                [0b01] = r => r.DE,
                [0b10] = r => r.IY,
                [0b11] = r => r.SP,
            };

            var r = (opCode & 0b00110000) >> 4;
            var value = registerPointers[r](Z80.Registers);

            Z80.Registers.IY = Z80.Alu.Add(Z80.Registers.IY, value);
        }
    }
}
