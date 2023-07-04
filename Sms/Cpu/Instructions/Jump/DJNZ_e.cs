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
                var e = (sbyte)Z80.Memory[Z80.Registers.PC++];

                Z80.Registers.PC += (ushort)e;

                cycles = 13;
            }
            else
            {
                Z80.Registers.PC++;
                cycles = 8;
            }
        }

        public override string ToString(byte opCode)
        {
            var e = (sbyte)Z80.Memory[(ushort)(Z80.Registers.PC + 1)] + 2;

            return $"djnz 0x{e + Z80.Registers.PC:x}";
        }
    }
}
