namespace Sms.Cpu.Instructions.CallAndReturn
{
    public class CALL_cc_nn : Instruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; }

        public CALL_cc_nn(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b11000100;
            var ccValues = Enumerable.Range(0, 8);

            OpCodes = ccValues.Select(cc => (byte)(opCodeBase | (cc << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var cc = (opCode & 0b00111000) >> 3;

            if (Z80.Alu.CheckFlags(cc))
            {
                var nn = Z80.Memory.ReadWord(Z80.Registers.PC);
                Z80.Registers.PC += 2;

                var pcHigh = (byte)((Z80.Registers.PC & 0xFF00) >> 8);
                var pcLow = (byte)(Z80.Registers.PC & 0x00FF);

                Z80.Memory[--Z80.Registers.SP] = pcHigh;
                Z80.Memory[--Z80.Registers.SP] = pcLow;
                Z80.Registers.PC = nn;

                cycles = 17;
            }
            else
            {
                Z80.Registers.PC += 2;
                cycles = 10;
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

            return $"call {condition}, 0x{nn:x}";
        }
    }
}
