namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class INI : EdInstruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b10100010 };

        public INI(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Alu.Ini();
        }
    }
}
