namespace Sms.Cpu
{
    public abstract class FdInstruction : Instruction
    {
        public FdInstruction(Z80 z80) : base(z80) { }
    }
}
