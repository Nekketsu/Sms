namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__nn__A : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; } = { 0b00110010 };

        public LD__nn__A(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n1 = Z80.Memory[Z80.Registers.PC++];
            var n2 = Z80.Memory[Z80.Registers.PC++];
            var nn = (ushort)((n1 << 8) | n2);

            Z80.Memory[nn] = Z80.Registers.A;
        }
    }
}
