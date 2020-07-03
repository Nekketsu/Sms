namespace Sms.Cpu.Instructions.Jump
{
    public class DJNZ_e : Instruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; } = { 0b00010000 };

        public DJNZ_e(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.B--;

            if (Z80.Registers.B != 0)
            {
                var e = Z80.Memory[Z80.Registers.PC++] - 126;

                Z80.Registers.PC = (ushort)(Z80.Registers.PC + e);

                cycles = 13;
            }
            else
            {
                cycles = 8;
            }
        }
    }
}
