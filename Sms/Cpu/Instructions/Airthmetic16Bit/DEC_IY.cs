namespace Sms.Cpu.Instructions.Airthmetic16Bit
{
    public class DEC_IY : FdInstruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b00101011 };

        public DEC_IY(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.IY = Z80.Alu.Dec(Z80.Registers.IY);
        }
    }
}
