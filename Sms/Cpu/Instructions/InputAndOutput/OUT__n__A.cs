namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class OUT__n__A : Instruction
    {
        public override uint Cycles => 11;
        public override byte[] OpCodes { get; } = { 0b11010011 };

        public OUT__n__A(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Ports[n] = Z80.Registers.A;
        }
    }
}
