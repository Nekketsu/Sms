namespace Sms.Cpu.Instructions.RotateAndShift
{
    public class SL1__IX_d__r : DdCbInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; }

        public SL1__IX_d__r(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b00110000;

            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;
            var d = (sbyte)Z80.Memory[(ushort)(Z80.Registers.PC - 2)];

            var value = Z80.Memory[(ushort)(Z80.Registers.IX + d)];
            var cy = value.HasBit(7);

            value = (byte)((value << 1) | 0x1);

            Z80.Memory[(ushort)(Z80.Registers.IX + d)] = value;
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
