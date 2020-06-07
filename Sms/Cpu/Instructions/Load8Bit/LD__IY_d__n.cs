namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__IY_d__n : FdInstruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; } = { 0b00110110 };

        public LD__IY_d__n(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var d = Z80.Memory[Z80.Registers.PC++];
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Memory[(ushort)(Z80.Registers.IY + d)] = n;
        }
    }
}
