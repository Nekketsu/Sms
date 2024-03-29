﻿namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD_SP_IY : FdInstruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b11111001 };

        public LD_SP_IY(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.SP = Z80.Registers.IY;
        }

        public override string ToString(byte opCode)
        {
            return "ld sp, iy";
        }
    }
}
