namespace Sms.Cpu
{
    public abstract class CbInstruction : Instruction
    {
        public CbInstruction(Z80 z80) : base(z80) { }
    }
}
