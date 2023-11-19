using Sms.Vdp;

namespace Sms.Cpu.Instructions.Arithmetic8Bit
{
    public class INC_r_IY : FdInstruction
    {
        public override uint Cycles => 4;
        public override byte[] OpCodes { get; }

        RegisterCollection<byte> registers;

        public INC_r_IY(Z80 z80) : base(z80)
        {
            registers = new RegisterCollection<byte>(z80.Registers, new()
            {
                [0x04] = r => r.B,
                [0x14] = r => r.D,
                [0x24] = r => r.IYH,
                [0x0c] = r => r.C,
                [0x1c] = r => r.E,
                [0x2c] = r => r.IYL,
                [0x3c] = r => r.A
            });

            OpCodes = registers.Indices.Select(i => (byte)i).ToArray();
        }

        protected override void InnerExecute(byte opCode)
        {
            registers[opCode] = Z80.Alu.Inc(registers[opCode]);
        }

        public override string ToString(byte opCode)
        {
            return $"inc {registers.Names[opCode]}";
        }
    }
}
