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
            cycles = 0;

            var output = new List<byte>();

            do
            {
                output.Add(Z80.Memory[Z80.Registers.HL]);

                Z80.Memory[Z80.Registers.DE] = Z80.Memory[Z80.Registers.HL];
                Z80.Registers.DE++;
                Z80.Registers.HL++;
                Z80.Registers.BC--;

                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, Z80.Registers.BC == 0);
                Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);

                if (Z80.Registers.BC != 0)
                {
                    cycles += 21;
                }
                else
                {
                    cycles += 16;
                }
            } while (Z80.Registers.BC != 0);
        }

        public override string ToString(byte opCode)
        {
            return "ldir";
        }
    }
}
