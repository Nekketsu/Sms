namespace Sms.Cpu.Instructions.Load16Bit
{
    public class PUSH_IX : DdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b11100101 };

        public PUSH_IX(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.SP -= 2;
            Z80.Memory.WriteWord(Z80.Registers.SP, Z80.Registers.IX);
        }
    }
}
