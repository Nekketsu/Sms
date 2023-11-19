namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class DEC_r_IY : FdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; }

        RegisterCollection<byte> registers;

        public DEC_r_IY(Z80 z80) : base(z80)
        {
            registers = new RegisterCollection<byte>(z80.Registers, new ()
            {
                [0x05] = r => r.B,
                [0x15] = r => r.D,
                [0x25] = r => r.IYH,
                [0x0d] = r => r.C,
                [0x1d] = r => r.E,
                [0x2d] = r => r.IYL,
                [0x3d] = r => r.A
            });

            OpCodes = registers.Indices.Select(i => (byte)i).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            registers[opCode] = Z80.Alu.Dec(registers[opCode]);
        }

        public override string ToString(byte opCode)
        {
            return $"dec {registers.Names[opCode]}";
        }
    }
}
