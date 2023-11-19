namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_B_r_IX : DdInstruction
    {
        public override uint Cycles => 9;
        public override byte[] OpCodes { get; }

        public LD_B_r_IX(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01000000;
            var indices = z80.Alu.Registers8BitIX.Indices;

            OpCodes = indices.SelectMany(r1 => indices.Select(r2 => (byte)(opCodeBase | r1 << 3 | r2))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var rDestination = (opCode & 0b00111000) >> 3;
            var rSource = opCode & 0b00000111;

            Z80.Alu.Registers8BitIX[rDestination] = Z80.Alu.Registers8BitIX[rSource];
        }
    }
}
