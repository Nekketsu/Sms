namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class LDDR : EdInstruction
    {
        private uint cycles;
        public override uint Cycles => cycles; // 21, 16
        public override byte[] OpCodes { get; } = { 0b10111000 };

        public LDDR(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Alu.Ldd();

            if (Z80.Registers.BC != 0)
            {
                cycles = 21;
                Z80.Registers.PC -= 2;
            }
            else
            {
                cycles = 16;

                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, false);
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
            }
        }
    }
}
