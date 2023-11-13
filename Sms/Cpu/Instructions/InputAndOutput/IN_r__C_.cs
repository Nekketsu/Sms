namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class IN_r__C_ : EdInstruction
    {
        public override uint Cycles => 12;
        public override byte[] OpCodes { get; }

        public IN_r__C_(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01000000;

            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | (r << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = (opCode & 0b00111000) >> 3;
            var value = Z80.Ports[Z80.Registers.C];

            if (r == 0b110)
            {
                Z80.Registers.F = (Registers.Flags)value;
            }
            else
            {
                Z80.Alu.Registers8Bit[r] = value;

                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value.HasEvenParity());
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            }
        }
    }
}
