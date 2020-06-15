namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class CP_n : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; } = { 0b11111110 };
        public CP_n(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Alu.Compare(n);
        }
    }
}
