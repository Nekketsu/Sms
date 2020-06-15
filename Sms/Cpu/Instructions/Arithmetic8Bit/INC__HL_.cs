namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class INC__HL_ : Instruction
    {
        public override uint Cycles => 11;
        public override byte[] OpCodes { get; } = { 0b00110100 };
        public INC__HL_(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            var value = Z80.Memory[Z80.Registers.HL];

            Z80.Memory[Z80.Registers.HL] = Z80.Alu.Inc(value);
        }
    }
}
