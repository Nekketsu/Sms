namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class EX__SP__IY : FdInstruction
    {
        public override uint Cycles => 23;
        public override byte[] OpCodes { get; } = { 0b11100011 };

        public EX__SP__IY(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory.ReadWord(Z80.Registers.SP);
            Z80.Memory.WriteWord(Z80.Registers.SP, Z80.Registers.IY);
            Z80.Registers.IY = value;
        }
    }
}
