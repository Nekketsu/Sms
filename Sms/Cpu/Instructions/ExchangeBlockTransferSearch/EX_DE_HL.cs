namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class EX_DE_HL : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b11101011 };

        public EX_DE_HL(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            (Z80.Registers.DE, Z80.Registers.HL) = (Z80.Registers.HL, Z80.Registers.DE);
        }
    }
}
