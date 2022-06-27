namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class CALL_cc_nn : Instruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; }

        public CALL_cc_nn(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000100;
            var ccValues = Enumerable.Range(0, 8);

            OpCodes = ccValues.Select(cc => (byte)(opCodeBase | (cc << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var cc = (opCode & 0b00111000) >> 3;

            if (Z80.Alu.CheckFlags(cc))
            {
                var n1 = Z80.Memory[Z80.Registers.PC++];
                var n2 = Z80.Memory[Z80.Registers.PC++];

                var nn = (ushort)((n1 << 8) | n2);

                var pcHigh = (byte)((Z80.Registers.PC & 0xFF00) >> 8);
                var pcLow = (byte)(Z80.Registers.PC & 0x00FF);

                Z80.Memory[--Z80.Registers.SP] = pcHigh;
                Z80.Memory[--Z80.Registers.SP] = pcLow;
                Z80.Registers.PC = nn;

                cycles = 17;
            }
            else
            {
                cycles = 10;
            }
        }
    }
}
