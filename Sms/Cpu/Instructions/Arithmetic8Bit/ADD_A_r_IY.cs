﻿namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class ADD_A_r_IY : FdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; }
        public ADD_A_r_IY(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b10000000;

            OpCodes = Z80.Alu.Registers8BitIY.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;
            var value = Z80.Alu.Registers8BitIY[r];

            Z80.Alu.Add(value);
        }

        public override string ToString(byte opCode)
        {
            var r = opCode & 0b00000111;
            var register = Z80.Alu.Registers8BitIY.Names[r];

            return $"add a, {register}";
        }
    }
}
