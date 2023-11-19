﻿namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class XOR_r_IX : DdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; }
        public XOR_r_IX(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b10101000;

            OpCodes = Z80.Alu.Registers8BitIX.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;
            var value = Z80.Alu.Registers8BitIX[r];

            Z80.Alu.Xor(value);
        }

        public override string ToString(byte opCode)
        {
            var r = opCode & 0b00000111;
            var register = Z80.Alu.Registers8BitIX.Names[r];

            return $"xor {register}";
        }
    }
}
