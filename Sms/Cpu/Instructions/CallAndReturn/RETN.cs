namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class RETN : EdInstruction
    {
        public override uint Cycles => 14;
        public override byte[] OpCodes { get; } = { 0b01000101 };

        public RETN(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var pcLow = Z80.Memory[Z80.Registers.SP++];
            var pcHigh = Z80.Memory[Z80.Registers.SP++];

            Z80.Registers.PC = (ushort)((pcHigh << 8) | pcLow);
            Z80.Registers.IFF1 = Z80.Registers.IFF2;
        }
    }
}
