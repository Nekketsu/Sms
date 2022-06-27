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
                var n1 = Z80.Memory[Z80.Registers.PC++];
                var n2 = Z80.Memory[Z80.Registers.PC++];

                var nn = (ushort)((n1 << 8) | n2);

                Z80.Registers.PC = nn;
            }
        }
    }
}
