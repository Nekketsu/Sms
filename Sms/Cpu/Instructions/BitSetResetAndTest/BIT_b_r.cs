using System.Linq;

namespace Sms.Cpu.Instructions.BitSetResetAndTest
{
    public class BIT_b_r : CbInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; }

        public BIT_b_r(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01000000;
            var bValues = Enumerable.Range(0, 8);
            var indices = z80.Alu.Registers8Bit.Indices;

            OpCodes = bValues.SelectMany(b => indices.Select(r => (byte)(opCodeBase | b << 3 | r))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var b = (opCode & 0b00111000) >> 3;
            var r = opCode & 0b00000111;

            var value = Z80.Alu.Registers8Bit[r];

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value.HasBit(b));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
        }
    }
}
