namespace Sms.Cpu.Instructions.Load8Bit
{
    public class LD_r_n_IY : FdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; }

        RegisterCollection<byte> registers;

        public LD_r_n_IY(Z80 z80) : base(z80)
        {
            registers = new RegisterCollection<byte>(z80.Registers, new()
            {
                [0x06] = r => r.B,
                [0x16] = r => r.D,
                [0x26] = r => r.IYH,
                [0x0e] = r => r.C,
                [0x1e] = r => r.E,
                [0x2e] = r => r.IYL,
                [0x3e] = r => r.A
            });

            OpCodes = registers.Indices.Select(i => (byte)i).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            var n = Z80.Memory[Z80.Registers.PC++];

            registers[opCode] = n;
        }

        public override string ToString(byte opCode)
        {
            return $"ld {registers.Names[opCode]}, n";
        }
    }
}
