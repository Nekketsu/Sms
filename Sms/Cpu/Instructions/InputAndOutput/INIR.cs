namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class INIR : EdInstruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; } = { 0b10110010 };

        public INIR(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Memory[Z80.Registers.HL] = Z80.Ports[Z80.Registers.C];
            Z80.Registers.B = (byte)((Z80.Registers.B - 1) % 256);
            Z80.Registers.HL++;

            if (Z80.Registers.B != 0)
            {
                Z80.Registers.PC -= 2;

                cycles = 21;
            }
            else
            {
                cycles = 16;
            }

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, true);
        }
    }
}
