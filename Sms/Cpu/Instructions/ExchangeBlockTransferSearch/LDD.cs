namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class LDD : EdInstruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b10101000 };

        public LDD(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Alu.Ldd();
        }
    }
}
