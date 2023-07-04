namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class CALL_nn : Instruction
    {
        public override uint Cycles => 17;
        public override byte[] OpCodes { get; } = { 0b11001101 };

        public CALL_nn(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
            Z80.Registers.PC += 2;

            var pcHigh = (byte)((Z80.Registers.PC & 0xFF00) >> 8);
            var pcLow = (byte)(Z80.Registers.PC & 0x00FF);

            Z80.Memory[--Z80.Registers.SP] = pcHigh;
            Z80.Memory[--Z80.Registers.SP] = pcLow;
            Z80.Registers.PC = nn;
        }

        public override string ToString(byte opCode)
        {
            var nn = Z80.Memory.ReadWord((ushort)(Z80.Registers.PC + 1));

            return $"call 0x{nn:x}";
        }
    }
}
