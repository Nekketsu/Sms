namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class INDR : EdInstruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; } = { 0b10111010 };

        public INDR(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Alu.Ind();

            if (Z80.Registers.B != 0)
            {
                Z80.Registers.PC -= 2;

                cycles = 21;
            }
            else
            {
                cycles = 16;
            }
        }
    }
}
