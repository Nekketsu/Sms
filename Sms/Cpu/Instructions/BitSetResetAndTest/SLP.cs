namespace Sms.Cpu.Instructions.BitSetResetAndTest
{
    public class SLP : EdInstruction
    {
        public override uint Cycles => 8;
        public override byte[] OpCodes { get; } = { 0b01110110 };

        public SLP(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {

        }

        public override string ToString(byte opCode)
        {
            return "slp";
        }
    }
}
