namespace Sms.Cpu.Instructions.Load16Bit
{
    public class LD_HL__nn_ : Instruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b00101010 };

        public LD_HL__nn_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n1 = Z80.Memory[Z80.Registers.PC++];
            var n2 = Z80.Memory[Z80.Registers.PC++];
            var nn = (ushort)((n1 << 8) | n2);

            Z80.Registers.HL = Z80.Memory.ReadWord(nn);
        }
    }
}
