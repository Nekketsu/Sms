namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class EXX : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b11011001 };

        public EXX(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            (Z80.Registers.BC, Z80.Registers.BCShadow) = (Z80.Registers.BCShadow, Z80.Registers.BC);
            (Z80.Registers.DE, Z80.Registers.DEShadow) = (Z80.Registers.DEShadow, Z80.Registers.DE);
            (Z80.Registers.HL, Z80.Registers.HLShadow) = (Z80.Registers.HLShadow, Z80.Registers.HL);
        }
    }
}
