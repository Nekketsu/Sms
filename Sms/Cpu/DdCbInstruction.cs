namespace Sms.Cpu
{
    public abstract class DdCbInstruction : Instruction
    {
        public DdCbInstruction(Z80 z80) : base(z80) { }
    }
}
