﻿namespace Sms.Cpu.Instructions.Airthmetic16Bit
{
    public class ADC_HL_ss : EdInstruction
    {
        public override uint Cycles => 15;

        public override byte[] OpCodes { get; }

        public ADC_HL_ss(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01001010;

            OpCodes = Z80.Alu.Registers16Bit.Indices.Select(r => (byte)(opCodeBase | (r << 4))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = (opCode & 0b00110000) >> 4;
            var value = Z80.Alu.Registers16Bit[r];
            var CY = Z80.Registers.F.HasFlag(Registers.Flags.C);

            Z80.Registers.HL = Z80.Alu.Add(Z80.Registers.HL, value, CY);
        }
    }
}
