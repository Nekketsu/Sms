namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD_IX_nn : DdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00100001 };

        public LD_IX_nn(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            Z80.Registers.IX = nn;
        }
    }
}
