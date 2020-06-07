namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__HL__n : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b01110000 };

        public LD__HL__n(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Memory[Z80.Registers.HL] = n;
        }
    }
}
