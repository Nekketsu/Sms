namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class IND : EdInstruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b10101010 };

        public IND(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Alu.Ind();
        }
    }
}
