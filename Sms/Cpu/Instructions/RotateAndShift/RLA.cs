namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class RLA : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00010111 };

        public RLA(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var cy = Z80.Registers.A.HasBit(7);

            Z80.Registers.A = (byte)((Z80.Registers.A << 1) | (Z80.Registers.F.HasFlag(Registers.Flags.C) ? 1 : 0));

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
