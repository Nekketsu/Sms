namespace Sms.Cpu
{
    public abstract class FdCbInstruction : Instruction
    {
        public FdCbInstruction(Z80 z80) : base(z80) { }
    }
}
