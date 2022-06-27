namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class OUT__C__r : EdInstruction
    {
        public override uint Cycles => 12;
        public override byte[] OpCodes { get; }

        public OUT__C__r(Z80 z80) : base(z80)
        {
            var opCodeBase = (byte)0b01000001;

            OpCodes = Z80.Alu.Registers8Bit.Indices.Select(r => (byte)(opCodeBase | (r << 3))).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var r = (opCode & 0b00111000) >> 3;

            Z80.Ports[Z80.Registers.C] = Z80.Alu.Registers8Bit[r];
        }
    }
}
