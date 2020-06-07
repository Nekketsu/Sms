namespace Sms.Cpu.Instructions.Load16Bit
{
    public class POP_IX : DdInstruction
    {
        public override uint Cycles => 14;
        public override byte[] OpCodes { get; } = { 0b11100001 };

        public POP_IX(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.IX = Z80.Memory.ReadWord(Z80.Registers.SP);
            Z80.Registers.SP += 2;
        }
    }
}
