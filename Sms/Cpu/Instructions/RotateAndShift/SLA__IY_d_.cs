namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class SLA__IY_d_ : FdCbInstruction
    {
        public override uint Cycles => 23;
        public override byte[] OpCodes { get; } = { 0b00100110 };

        public SLA__IY_d_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = Z80.Memory[(ushort)(Z80.Registers.PC - 2)];

            var value = Z80.Memory[(ushort)(Z80.Registers.IY + d)];
            var cy = value.HasBit(7);

            value <<= 1;
            Z80.Memory[(ushort)(Z80.Registers.IY + d)] = value;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value % 2 == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
