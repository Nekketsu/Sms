namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class SBC_A_r : Instruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; }
        public SBC_A_r(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b10011000;

            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;
            var value = Z80.Alu.Registers8Bit[r];
            var CY = Z80.Registers.F.HasFlag(Registers.Flags.C);

            Z80.Alu.Sub(value, CY);
        }
    }
}
