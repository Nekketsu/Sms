using static Sms.Cpu.Registers;

namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class OUTD : EdInstruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b10101011 };

        public OUTD(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory[Z80.Registers.HL];
            Z80.Registers.B--;
            Z80.Ports[Z80.Registers.C] = value;
            Z80.Registers.HL--;

            var k = Z80.Registers.L + value;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, Z80.Registers.B.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, Z80.Registers.B == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, k > 255);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, ((ushort)(((k & 0x7)) ^ Z80.Registers.B)).HasEvenParity());
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, value.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, k > 255);
        }
    }
}
