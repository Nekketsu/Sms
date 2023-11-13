namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD__IX_d__r : DdInstruction
    {
        public override uint Cycles => 19;
        public override byte[] OpCodes { get; }

        public LD__IX_d__r(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01110000;
            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | r)).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = opCode & 0b00000111;
            var d = (sbyte)Z80.Memory[Z80.Registers.PC++];

            Z80.Memory[(ushort)(Z80.Registers.IX + d)] = Z80.Alu.Registers8Bit[r];
        }
    }
}
