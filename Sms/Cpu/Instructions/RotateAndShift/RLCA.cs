namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class RLCA : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00000111 };

        public RLCA(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var cy = Z80.Registers.A.HasBit(7);

            Z80.Registers.A = (byte)((Z80.Registers.A << 1) | (cy ? 1 : 0));

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
