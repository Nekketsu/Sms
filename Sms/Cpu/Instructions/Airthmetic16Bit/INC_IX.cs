﻿namespace Sms.Cpu.Instructions.Airthmetic16Bit
{
    public class INC_IX : DdInstruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b00100011 };

        public INC_IX(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.IX = Z80.Alu.Inc(Z80.Registers.IX);
        }
    }
}
