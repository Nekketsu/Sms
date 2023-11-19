namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class SL1__HL_ : CbInstruction
    {
        public override uint Cycles => 15;
        public override byte[] OpCodes { get; } = { 0b00110110 };

        public SL1__HL_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory[Z80.Registers.HL];
            var cy = value.HasBit(7);

            value = (byte)((value << 1) | 0x1);
            Z80.Memory[Z80.Registers.HL] = value;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value.HasEvenParity());
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
