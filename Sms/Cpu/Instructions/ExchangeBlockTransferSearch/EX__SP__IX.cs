namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class EX__SP__IX : DdInstruction
    {
        public override uint Cycles => 23;
        public override byte[] OpCodes { get; } = { 0b11100011 };

        public EX__SP__IX(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory.ReadWord(Z80.Registers.SP);
            Z80.Memory.WriteWord(Z80.Registers.SP, Z80.Registers.IX);
            Z80.Registers.IX = value;
        }
    }
}
