namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class RRA : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00011111 };

        public RRA(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var cy = Z80.Registers.A.HasBit(0);

            Z80.Registers.A = (byte)((Z80.Registers.A >> 1) | (Z80.Registers.F.HasFlag(Registers.Flags.C) ? 0b10000000 : 0));

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
