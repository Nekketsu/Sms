﻿namespace Sms.Cpu.Instructions
{
    public class DD_Instruction : Instruction
    {
        public override uint Cycles => 0;
        public override byte[] OpCodes { get; } = { 0xCB };

        public DD_Instruction(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nextOpCode = Z80.Memory[Z80.Registers.PC];

            Z80.ExecuteDdOpCodebyte(nextOpCode);
        }
    }
}
