namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_r_n : Instruction
    {
        public override uint Cycles => 7;
        public override byte[] OpCodes { get; }


        public LD_r_n(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b00000110;
            OpCodes = z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | (r << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = (opCode & 0b00111000) >> 3;
            var n = Z80.Memory[Z80.Registers.PC++];

            Z80.Alu.Registers8Bit[r] = n;
        }

        public override string ToString(byte opCode)
        {
            var r = (opCode & 0b00111000) >> 3;
            var n = Z80.Memory[(ushort)(Z80.Registers.PC + 1)];

            var register = Z80.Alu.Registers8Bit.Names[r];

            return $"ld {register}, 0x{n:x}";
        }
    }
}
