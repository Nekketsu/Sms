namespace Sms.Cpu.Instructions.Jump
{
    public class JR_Z_e : Instruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; } = { 0b00101000 };

        public JR_Z_e(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var condition = Z80.Registers.F.HasFlag(Registers.Flags.Z);

            if (condition)
            {
                var e = (sbyte)Z80.Memory[Z80.Registers.PC++];

                Z80.Registers.PC += (ushort)e;

                cycles = 12;
            }
            else
            {
                Z80.Registers.PC++;

                cycles = 7;
            }
        }

        public override string ToString(byte opCode)
        {
            var e = (sbyte)Z80.Memory[(ushort)(Z80.Registers.PC + 1)] + 2;

            return $"jr z, 0x{e + Z80.Registers.PC:x}";
        }
    }
}
