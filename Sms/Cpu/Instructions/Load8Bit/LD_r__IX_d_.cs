﻿using System.Linq;

namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_r__IX_d_ : DdInstruction
    {
        public override uint Cycles => 5;
        public override byte[] OpCodes { get; }

        public LD_r__IX_d_(Z80 z80) : base(z80)
        {
            var opCodeBase = 0b01000110;

            OpCodes = z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | (r << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var destination = (opCode & 0b00111000) >> 3;
            var d = (sbyte)Z80.Memory[Z80.Registers.PC++];
            var value = Z80.Memory[(ushort)(Z80.Registers.IX + d)];

            Z80.Alu.Registers8Bit[destination] = value;
        }
    }
}