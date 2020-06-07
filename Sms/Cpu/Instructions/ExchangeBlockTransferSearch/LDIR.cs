namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class LDIR : EdInstruction
    {
        private uint cycles;
        public override uint Cycles => cycles; // 21, 16
        public override byte[] OpCodes { get; } = { 0b10110000 };

        public LDIR(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            cycles = Z80.Registers.BC != 0 ? 16u : 21u;

            var value = Z80.Memory.ReadWord(Z80.Registers.HL);
            Z80.Memory.WriteWord(Z80.Registers.DE, value);
            Z80.Registers.DE++;
            Z80.Registers.HL++;
            Z80.Registers.BC--;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, Z80.Registers.BC == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);

            if (Z80.Registers.BC != 0)
            {
                Z80.Registers.PC -= 2;
            }
        }
    }
}
