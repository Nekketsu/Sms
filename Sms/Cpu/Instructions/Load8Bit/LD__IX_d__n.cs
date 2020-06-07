namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__IX_d__n : DdInstruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; } = { 0b00110110 };

        public LD__IX_d__n(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = Z80.Memory[Z80.Registers.PC++];
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Memory[(ushort)(Z80.Registers.IX + d)] = n;
        }
    }
}
