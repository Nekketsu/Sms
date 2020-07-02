﻿namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class RL__IX_n_ : DdCbInstruction
    {
        public override uint Cycles => 23;
        public override byte[] OpCodes { get; } = { 0b00010110 };

        public RL__IX_n_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = Z80.Memory[(ushort)(Z80.Registers.PC - 2)];

            var value = Z80.Memory[(ushort)(Z80.Registers.IX + d)];
            var cy = value.HasBit(7);

            value = (byte)((value << 1) | (Z80.Registers.F.HasFlag(Registers.Flags.C) ? 1 : 0));
            Z80.Memory[(ushort)(Z80.Registers.IX + d)] = value;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value % 2 == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
