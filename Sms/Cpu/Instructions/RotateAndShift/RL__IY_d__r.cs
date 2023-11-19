﻿namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class RL__IY_d__r : FdCbInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; }

        public RL__IY_d__r(Z80 z80) : base(z80)
        {
            var opCodeBase = 0b00010000;

            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;
            var d = (sbyte)Z80.Memory[(ushort)(Z80.Registers.PC - 2)];

            var value = Z80.Memory[(ushort)(Z80.Registers.IY + d)];
            var cy = value.HasBit(7);

            value = (byte)((value << 1) | (Z80.Registers.F.HasFlag(Registers.Flags.C) ? 1 : 0));

            Z80.Memory[(ushort)(Z80.Registers.IY + d)] = value;
            Z80.Alu.Registers8Bit[r] = value;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value.HasEvenParity());
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
