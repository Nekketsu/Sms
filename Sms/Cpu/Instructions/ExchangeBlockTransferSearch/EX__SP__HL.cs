namespace Sms.Cpu.Instructions.ExchangeBlockTransferSearch
{
    public class EX__SP__HL : Instruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; } = { 0b11100011 };

        public EX__SP__HL(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            (Z80.Registers.H, Z80.Memory[(ushort)(Z80.Registers.SP + 1)]) = (Z80.Memory[(ushort)(Z80.Registers.SP + 1)], Z80.Registers.H);
            (Z80.Registers.L, Z80.Memory[Z80.Registers.SP]) = (Z80.Memory[Z80.Registers.SP], Z80.Registers.L);
        }
    }
}
