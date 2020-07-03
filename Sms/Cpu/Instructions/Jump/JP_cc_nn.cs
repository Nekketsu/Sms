using System;
using System.Linq;

namespace Sms.Cpu.Instructions.Jump
{
    public class JP_cc_nn : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; }

        public JP_cc_nn(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000010;
            var ccValues = Enumerable.Range(0, 8);

            OpCodes = ccValues.Select(cc => (byte)(opCodeBase | (cc << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var cc = (opCode & 0b00111000) >> 3;

            var n1 = Z80.Memory[Z80.Registers.PC++];
            var n2 = Z80.Memory[Z80.Registers.PC++];

            var n = (ushort)((n1 << 8) | n2);

            var condition = cc switch
            {
                0b000 => !Z80.Registers.F.HasFlag(Registers.Flags.Z),
                0b001 => Z80.Registers.F.HasFlag(Registers.Flags.Z),
                0b010 => !Z80.Registers.F.HasFlag(Registers.Flags.C),
                0b011 => Z80.Registers.F.HasFlag(Registers.Flags.C),
                0b100 => !Z80.Registers.F.HasFlag(Registers.Flags.PV),
                0b101 => Z80.Registers.F.HasFlag(Registers.Flags.PV),
                0b110 => !Z80.Registers.F.HasFlag(Registers.Flags.S),
                0b111 => Z80.Registers.F.HasFlag(Registers.Flags.S),
                _ => throw new NotImplementedException()
            };

            if (condition)
            {
                Z80.Registers.PC = n;
            }
        }
    }
}
