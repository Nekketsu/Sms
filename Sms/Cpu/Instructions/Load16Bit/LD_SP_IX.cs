namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD_SP_IX : DdInstruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b11111001 };

        public LD_SP_IX(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.SP = Z80.Registers.IY;
        }
    }
}
