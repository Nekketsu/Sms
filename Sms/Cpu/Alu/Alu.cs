using static Sms.Cpu.Registers;

namespace Sms.Cpu
{
    public class Alu
    {
        private Z80 Z80 { get; }
        public Registers8Bit Registers8Bit { get; }
        public Registers16Bit Registers16Bit { get; }

        public Alu(Z80 z80)
        {
            Z80 = z80;
            Registers8Bit = new Registers8Bit(z80.Registers);
            Registers16Bit = new Registers16Bit(z80.Registers);
        }

        public int Exchange(ref ushort ex1, ref ushort ex2)
        {
            var opCodeCyles = 4;

            (ex1, ex2) = (ex2, ex1);

            return opCodeCyles;
        }

        public int JumpImmediate(bool useCondition, Flags flag, bool condition)
        {
            var opCodeCycles = 12;

            if (!useCondition)
            {
                sbyte n = (sbyte)Z80.Memory[Z80.Registers.PC];

                Z80.Registers.PC = (ushort)(Z80.Registers.PC + n);
            }
            else if (Z80.Registers.F.HasFlag(flag) == condition)
            {
                sbyte n = (sbyte)Z80.Memory[Z80.Registers.PC];

                Z80.Registers.PC = (ushort)(Z80.Registers.PC + n);
            }
            else
            {
                opCodeCycles = 7;
            }

            Z80.Registers.PC++;

            return opCodeCycles;
        }

        public int Decrement8Bit(ref byte register, int cycles)
        {
            var opCodeCyles = cycles;

            byte before = register;

            register--;

            var flags = Z80.Registers.F;

            // Set Z flag if result is negative
            flags = flags.SetFlags(Flags.Z, register == 0);

            // Set H flag if lower nibble is 0, meaning it will carry from bit 4
            flags = flags.SetFlags(Flags.H, (before & 0x0F) == 0);

            // V is calculated not P
            flags = flags.SetFlags(Flags.PV, (sbyte)before == -128);

            // Set substract flag
            flags |= Flags.N;

            // Set sign flag to bit 7 of the result
            flags.SetFlags(Flags.S, register.HasBit(7));

            Z80.Registers.F = flags;

            return opCodeCyles;
        }

        public void PushWordOnStack(ushort word)
        {
            var hi = (byte)(word >> 8);
            var lo = (byte)(word & 0xFF);

            Z80.Registers.SP--;
            Z80.Memory[Z80.Registers.SP] = hi;
            Z80.Registers.SP--;
            Z80.Memory[Z80.Registers.SP] = lo;
        }
    }
}
