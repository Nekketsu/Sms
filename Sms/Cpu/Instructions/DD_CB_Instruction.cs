namespace Sms.Cpu.Instructions
{
    public class DD_CB_Instruction : DdInstruction
    {
        public override uint Cycles => 0;
        public override byte[] OpCodes { get; } = { 0xCB };

        public DD_CB_Instruction(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            Z80.Registers.R = (byte)((Z80.Registers.R + 127) % 128);

            Z80.Registers.PC++; // var d = Z80.Memory[Z80.Registers.PC++];
            var nextOpCode = Z80.Memory[Z80.Registers.PC];

            Z80.ExecuteDdCbOpCodebyte(nextOpCode);
        }
    }
}
