namespace Sms.Cpu.Instructions.Jump
{
    public class JR_e : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b00011000 };

        public JR_e(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var e = Z80.Memory[Z80.Registers.PC++] - 126;

            Z80.Registers.PC = (ushort)(Z80.Registers.PC + e);
        }
    }
}
