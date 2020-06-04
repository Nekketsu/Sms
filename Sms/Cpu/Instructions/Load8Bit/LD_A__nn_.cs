namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_A__nn_ : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00111010 };

        public LD_A__nn_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n1 = Z80.Memory[Z80.Registers.PC++];
            var n2 = Z80.Memory[Z80.Registers.PC++];
            var nn = (ushort)((n1 << 8) | n2);

            Z80.Registers.A = Z80.Memory[nn];
        }
    }
}
