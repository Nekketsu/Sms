namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class RET_cc : Instruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; }

        public RET_cc(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000000;
            var ccValues = Enumerable.Range(0, 8);

            OpCodes = ccValues.Select(cc => (byte)(opCodeBase | (cc << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var cc = (opCode & 0b00111000) >> 3;

            if (Z80.Alu.CheckFlags(cc))
            {
                var pcLow = ++Z80.Registers.SP;
                var pcHigh = ++Z80.Registers.SP;

                Z80.Registers.PC = (byte)((pcHigh << 4) | pcLow);

                cycles = 11;
            }
            else
            {
                cycles = 5;
            }
        }
    }
}
