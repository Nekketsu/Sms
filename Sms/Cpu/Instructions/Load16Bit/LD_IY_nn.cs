namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD_IY_nn : FdInstruction
    {
        public override uint Cycles => 14;
        public override byte[] OpCodes { get; } = { 0b00100001 };

        public LD_IY_nn(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            Z80.Registers.IY = nn;
        }
    }
}
