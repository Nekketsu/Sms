namespace Sms.Cpu
{
    public abstract class DdInstruction : Instruction
    {
        public DdInstruction(Z80 z80) : base(z80) { }
    }
}
