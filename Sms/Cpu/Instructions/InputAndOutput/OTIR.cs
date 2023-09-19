using System.Diagnostics;
using System.Text;

namespace Sms.Cpu.Instructions.InputAndOutput
{
    public class OTIR : EdInstruction
    {
        private uint cycles;
        public override uint Cycles => cycles;
        public override byte[] OpCodes { get; } = { 0b10110011 };

        public OTIR(Z80 z80) : base(z80) { }

        protected override void InnerExecute(byte opCode)
        {
            cycles = 0;

            var output = new List<byte>();

            do
            {
                output.Add(Z80.Memory[Z80.Registers.HL]);

                Z80.Ports[Z80.Registers.C] = Z80.Memory[Z80.Registers.HL];
                Z80.Registers.B = (byte)((Z80.Registers.B - 1) % 256);
                Z80.Registers.HL++;

                if (Z80.Registers.B != 0)
                {
                    cycles += 21;
                }
                else
                {
                    Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.Z, true);
                    Z80.Registers.F = Z80.Registers.F.SetFlags(Registers.Flags.N, true);

                    cycles += 16;
                }
            } while (Z80.Registers.B != 0);
        }

        public override string ToString(byte opCode)
        {
            return "otir";
        }
    }
}
