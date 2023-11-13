namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_A_I : EdInstruction
    {
        public override uint Cycles => 9;
        public override byte[] OpCodes { get; } = { 0b01010111 };

        public LD_A_I(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.A = Z80.Registers.I;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, (Z80.Registers.I & (1 << 7)) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, Z80.Registers.I == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, Z80.Registers.IFF2);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
        }
    }
}
