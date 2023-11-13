namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class SRL_r : CbInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; }

        public SRL_r(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b00111000;

            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;

            var value = Z80.Alu.Registers8Bit[r];
            var cy = value.HasBit(0);

            value >>= 1;
            Z80.Alu.Registers8Bit[r] = value;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, value.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, value == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, value.HasEvenParity());
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.C, cy);
        }
    }
}
