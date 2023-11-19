namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class OUTD : EdInstruction
    {
        public override uint Cycles => 16;
        public override byte[] OpCodes { get; } = { 0b10101011 };

        public OUTD(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Alu.Outd();
        }
    }
}
