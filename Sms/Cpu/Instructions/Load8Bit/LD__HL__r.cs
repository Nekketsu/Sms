namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__HL__r : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; }

        public LD__HL__r(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01110000;

            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;

            Z80.Memory[Z80.Registers.HL] = Z80.Alu.Registers8Bit[r];
        }

        public override string ToString(byte opCode)
        {
            var r = opCode & 0b00000111;
            var register = Z80.Alu.Registers8Bit.Names[r];

            return $"ld (hl) {register}";
        }
    }
}
