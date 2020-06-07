namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class EX_AF_AFShadow : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00001000 };

        public EX_AF_AFShadow(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            (Z80.Registers.AF, Z80.Registers.AFShadow) = (Z80.Registers.AFShadow, Z80.Registers.AF);
        }
    }
}
