namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class IN_A__n_ : Instruction
    {
        public override uint Cycles => 11;
        public override byte[] OpCodes { get; } = { 0b11011011 };

        public IN_A__n_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Registers.A = Z80.Ports[n];
        }
    }
}
