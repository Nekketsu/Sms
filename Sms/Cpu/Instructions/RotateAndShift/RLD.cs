namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class RLD : EdInstruction
    {
        public override uint Cycles => 18;
        public override byte[] OpCodes { get; } = { 0b01101111 };

        public RLD(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory[Z80.Registers.HL];

            Z80.Memory[Z80.Registers.HL] = (byte)(((value & 0b00001111) << 4) | (Z80.Registers.A & 0b00001111));
            Z80.Registers.A = (byte)((Z80.Registers.A & 0b11110000) | ((value & 0b11110000) >> 4));

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, Z80.Registers.A % 2 == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
        }
    }
}
