namespace Sms.Cpu.Instructions.BitSetResetAndTest
{
    public class TSTIO : EdInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; } = { 0b01110100 };

        public TSTIO(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            var value = (byte)(n & Z80.Ports[Z80.Registers.C]);

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value.HasEvenParity());
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
        }

        public override string ToString(byte opCode)
        {
            return "tstio";
        }
    }
}
