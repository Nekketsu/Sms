﻿using System.Linq;

namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_r__HL_ : Instruction
    {
        public override uint Cycles => 2;
        public override byte[] OpCodes { get; }

        public LD_r__HL_(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01000110;

            OpCodes = z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | (r << 3))).ToArray();
        }


        protected override void InnerExecute(byte opCode)
        {
            var destination = (opCode & 0b00111000) << 3;
            var value = Z80.Memory[Z80.Registers.HL];

            Z80.Alu.Registers8Bit[destination] = value;
        }
    }
}