namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class RET : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b11001001 };

        public RET(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var pcLow = Z80.Memory[Z80.Registers.SP++];
            var pcHigh = Z80.Memory[Z80.Registers.SP++];

            Z80.Registers.PC = (ushort)((pcHigh << 8) | pcLow);
        }

        public override string ToString(byte opCode)
        {
            return "ret";
        }
    }
}
