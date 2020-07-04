namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class RET : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b11001001 };

        public RET(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var pcLow = ++Z80.Registers.SP;
            var pcHigh = ++Z80.Registers.SP;

            Z80.Registers.PC = (byte)((pcHigh << 4) | pcLow);
        }
    }
}
