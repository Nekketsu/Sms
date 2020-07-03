namespace Sms.Cpu.Instructions.Jump
{
    public class JR_NC_e : Instruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; } = { 0b00110000 };

        public JR_NC_e(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var condition = !Z80.Registers.F.HasFlag(Registers.Flags.C);

            if (condition)
            {
                var e = Z80.Memory[Z80.Registers.PC++] - 126;

                Z80.Registers.PC = (ushort)(Z80.Registers.PC + e);

                cycles = 12;
            }
            else
            {
                cycles = 7;
            }
        }
    }
}
