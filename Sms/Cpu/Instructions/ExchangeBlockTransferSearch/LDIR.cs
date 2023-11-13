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
            Z80.Alu.Ldi();

            if (Z80.Registers.BC != 0)
            {
                Z80.Registers.PC -= 2;
                cycles = 21;
            }
            else
            {
                cycles = 16;

                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, Z80.Registers.BC == 0);
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            }
        }

        public override string ToString(byte opCode)
        {
            return "ldir";
        }
    }
}
