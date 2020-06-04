namespace Sms.Cpu.Instructions.Load16Bit
{
    public class POP_IY : FdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b11100001 };

        public POP_IY(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.IY = Z80.Memory.ReadWord(Z80.Registers.SP);
            Z80.Registers.SP += 2;
        }
    }
}
