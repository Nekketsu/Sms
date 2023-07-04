namespace Sms.Cpu.Instructions.Jump
{
    public class JR_e : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b00011000 };

        public JR_e(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var e = (sbyte)Z80.Memory[Z80.Registers.PC++];

            Z80.Registers.PC += (ushort)e;
        }

        public override string ToString(byte opCode)
        {
            var e = (sbyte)Z80.Memory[(ushort)(Z80.Registers.PC + 1)] + 2;

            return $"jr 0x{e + Z80.Registers.PC:x}";
        }
    }
}
