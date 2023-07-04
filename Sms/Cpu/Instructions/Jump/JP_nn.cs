namespace Sms.Cpu.Instructions.Jump
{
    public class JP_nn : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; } = { 0b11000011 };

        public JP_nn(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var nn = Z80.Memory.ReadWord(Z80.Registers.PC);

            Z80.Registers.PC = nn;
        }

        public override string ToString(byte opCode)
        {
            var nn = Z80.Memory.ReadWord((ushort)(Z80.Registers.PC + 1));

            return $"jp 0x{nn:x}";
        }
    }
}
