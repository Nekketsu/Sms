namespace Sms.Cpu.Instructions.Jump
{
    public class JP_cc_nn : Instruction
    {
        public override uint Cycles => 10;
        public override byte[] OpCodes { get; }

        public JP_cc_nn(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000010;
            var ccValues = Enumerable.Range(0, 8);

            OpCodes = ccValues.Select(cc => (byte)(opCodeBase | (cc << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var cc = (opCode & 0b00111000) >> 3;

            if (Z80.Alu.CheckFlags(cc))
            {
                var nn = Z80.Memory.ReadWord(Z80.Registers.PC);

                Z80.Registers.PC = nn;
            }
            else
            {
                Z80.Registers.PC += 2;
            }
        }

        public override string ToString(byte opCode)
        {
            var cc = (opCode & 0b00111000) >> 3;
            var nn = Z80.Memory.ReadWord((ushort)(Z80.Registers.PC + 1));

            var condition = cc switch
            {
                0b000 => "nz",
                0b001 => "z",
                0b010 => "nc",
                0b011 => "c",
                0b100 => "po",
                0b101 => "pe",
                0b110 => "p",
                0b111 => "m",
                _ => throw new ArgumentException()
            };

            return $"jp {condition}, 0x{nn:x}";
        }
    }
}
