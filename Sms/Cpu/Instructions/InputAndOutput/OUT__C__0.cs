namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class OUT__C__0 : EdInstruction
    {
        public override uint Cycles => 12;
        public override byte[] OpCodes { get; } = { 0b01110001 };

        public OUT__C__0(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Ports[Z80.Registers.C] = 0;
        }

        public override string ToString(byte opCode)
        {
            return "out (c), 0";
        }
    }
}
