namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class CPDR : EdInstruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; } = { 0b10111001 };

        public CPDR(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory.ReadWord(Z80.Registers.HL);
            var result = Z80.Registers.A - value;
            Z80.Registers.HL--;
            Z80.Registers.BC--;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.S, result < 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, result == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, (Z80.Registers.A & 0b00001111) == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, Z80.Registers.BC == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, true);
            
            if (Z80.Registers.BC != 0 && result != 0)
            {
                Z80.Registers.PC -= 2;
            }

            cycles = Z80.Registers.BC != 0 && result != 0 ? 21u : 16u;
        }
    }
}
