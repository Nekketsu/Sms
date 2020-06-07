namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class LDD : EdInstruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b10101000 };

        public LDD(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory.ReadWord(Z80.Registers.HL);
            Z80.Memory.WriteWord(Z80.Registers.DE, value);
            Z80.Registers.DE--;
            Z80.Registers.HL--;
            Z80.Registers.BC--;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.PV, Z80.Registers.BC == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, false);
        }
    }
}
