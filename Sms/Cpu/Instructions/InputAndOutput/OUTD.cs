namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class OUTD : EdInstruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b10101011 };

        public OUTD(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Ports[Z80.Registers.C] = Z80.Memory[Z80.Registers.HL];
            Z80.Registers.B--;
            Z80.Registers.HL--;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, Z80.Registers.B == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, true);
        }
    }
}
