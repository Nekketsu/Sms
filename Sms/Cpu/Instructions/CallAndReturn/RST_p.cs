using System;
using System.Linq;

namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class RST_p : Instruction
    {
        public override uint Cycles => 11;
        public override byte[] OpCodes { get; }

        public RST_p(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000111;
            var ccValues = Enumerable.Range(0, 8);

            OpCodes = ccValues.Select(t => (byte)(opCodeBase | (t << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var t = (opCode & 0b00111000) >> 3;
            var p = (byte)(t switch
            {
                0b000 => 0x00,
                0b001 => 0x08,
                0b010 => 0x10,
                0b011 => 0x18,
                0b100 => 0x20,
                0b101 => 0x28,
                0b110 => 0x30,
                0b111 => 0x38,
                _ => throw new NotImplementedException()
            });

            Z80.Memory[--Z80.Registers.SP] = (byte)((Z80.Registers.PC & 0xFF00) >> 8);
            Z80.Memory[--Z80.Registers.SP] = (byte)((Z80.Registers.PC & 0xFF00) >> 8);
            Z80.Registers.PC = p;
        }
    }
}
