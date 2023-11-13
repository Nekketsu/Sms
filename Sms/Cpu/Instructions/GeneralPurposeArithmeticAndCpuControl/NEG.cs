namespace Sms.Cpu.Instructions.GeneralPurposeArithmeticAndCpuControl
{
    public class NEG : EdInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; } = { 0b01000100 };

        public NEG(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var before = Z80.Registers.A;

            Z80.Registers.A = (byte)(0 - Z80.Registers.A);

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, ((before ^ Z80.Registers.A) & (1 << 4)) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, before == 0x80);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, before != 0);
        }
    }
}
