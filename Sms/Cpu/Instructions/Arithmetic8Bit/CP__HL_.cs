﻿namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class CP__HL_ : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b10111110 };
        public CP__HL_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory[Z80.Registers.HL];

            Z80.Alu.Compare(value);
        }

        public override string ToString(byte opCode)
        {
            return "cp (hl)";
        }
    }
}
