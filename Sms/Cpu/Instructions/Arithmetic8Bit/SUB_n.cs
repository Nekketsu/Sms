﻿namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class SUB_n : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b11010110 };
        public SUB_n(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Alu.Sub(n);
        }
    }
}
