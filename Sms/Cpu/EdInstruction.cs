namespace Sms.Cpu
{
    public abstract class EdInstruction : Instruction
    {
        public EdInstruction(Z80 z80) : base(z80) { }
    }
}
