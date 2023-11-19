namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class IN__C_ : EdInstruction
    {
        public override uint Cycles => 12;
        public override byte[] OpCodes { get; } = { 0b01110000 };

        public IN__C_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Ports[Z80.Registers.C];

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value.HasEvenParity());
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
        }
    }
}
