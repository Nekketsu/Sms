namespace Sms.Cpu.Instructions.Jump
{
    public class JP_nn : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b11000011 };

        public JP_nn(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n1 = Z80.Memory[Z80.Registers.PC++];
            var n2 = Z80.Memory[Z80.Registers.PC++];

            var n = (ushort)((n1 << 8) | n2);

            Z80.Registers.PC = n;
        }
    }
}
