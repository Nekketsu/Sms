namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class CALL_nn : Instruction
    {
        public override uint Cycles => 17;
        public override byte[] OpCodes { get; } = { 0b11001101 };

        public CALL_nn(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n1 = Z80.Memory[Z80.Registers.PC++];
            var n2 = Z80.Memory[Z80.Registers.PC++];

            var nn = (ushort)((n1 << 8) | n2);

            var pcHigh = (byte)((Z80.Registers.PC & 0xFF00) >> 8);
            var pcLow = (byte)(Z80.Registers.PC & 0x00FF);

            Z80.Memory[Z80.Registers.SP--] = pcHigh;
            Z80.Memory[Z80.Registers.SP--] = pcLow;
            Z80.Registers.PC = nn;
        }
    }
}
