﻿namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class RR__IX_d_ : DdCbInstruction
    {
        public override uint Cycles => 23;
        public override byte[] OpCodes { get; } = { 0b00011110 };

        public RR__IX_d_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = (sbyte)Z80.Memory[(ushort)(Z80.Registers.PC - 2)];

            var value = Z80.Memory[(ushort)(Z80.Registers.IX + d)];
            var cy = value.HasBit(0);

            value = (byte)((value >> 1) | (Z80.Registers.F.HasFlag(Registers.Flags.C) ? 0b10000000 : 0));
            Z80.Memory[(ushort)(Z80.Registers.IX + d)] = value;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value.HasEvenParity());
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
