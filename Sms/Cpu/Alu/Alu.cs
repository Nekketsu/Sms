using System;
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

        public void Add(byte value, bool carry = false)
        {
            var before = Z80.Registers.A;
            var result = before + value + (carry ? 1 : 0);

            Z80.Registers.A = (byte)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, ((before ^ result) & 0xF) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, before.HasBit(7) == value.HasBit(7) && before.HasBit(7) != Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, result > 0xFF);
        }

        public ushort Add(ushort destination, ushort value)
        {
            var before = destination;
            var result = before + value;

            var ushortResult = (ushort)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, ((before ^ result) & 0xFFF) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, result > 0xFFFF);

            return ushortResult;
        }

        public ushort Add(ushort destination, ushort value, bool carry)
        {
            var before = destination;
            var result = before + value + (carry ? 1 : 0);

            var ushortResult = (ushort)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, ushortResult.HasBit(15));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, ushortResult == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, ((before ^ result) & 0xFFF) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, before.HasBit(15) == value.HasBit(15) && before.HasBit(15) != Z80.Registers.A.HasBit(15));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, result > 0xFFFF);

            return ushortResult;
        }

        public void Sub(byte value, bool carry = false)
        {
            var before = Z80.Registers.A;
            var result = before - value - (carry ? 1 : 0);

            Z80.Registers.A = (byte)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, ((before ^ result) & 0x10) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, before.HasBit(7) == value.HasBit(7) && before.HasBit(7) != Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, result < 0);
        }

        public ushort Sub(ushort destination, ushort value, bool carry = false)
        {
            var before = destination;
            var result = before - value - (carry ? 1 : 0);

            var ushortResult = (ushort)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, Z80.Registers.A.HasBit(15));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, ((before ^ result) & 0x1000) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, before.HasBit(15) == value.HasBit(15) && before.HasBit(15) != ushortResult.HasBit(15));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, result < 0);

            return ushortResult;
        }

        public void And(byte value)
        {
            var before = Z80.Registers.A;
            var result = before & value;

            Z80.Registers.A = (byte)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, before.HasBit(7) == value.HasBit(7) && before.HasBit(7) != Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, false);
        }

        public void Or(byte value)
        {
            var before = Z80.Registers.A;
            var result = before | value;

            Z80.Registers.A = (byte)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, before.HasBit(7) == value.HasBit(7) && before.HasBit(7) != Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, false);
        }

        public void Xor(byte value)
        {
            var before = Z80.Registers.A;
            var result = before ^ value;

            Z80.Registers.A = (byte)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, Z80.Registers.A == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, before.HasBit(7) == value.HasBit(7) && before.HasBit(7) != Z80.Registers.A.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, false);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, false);
        }

        public void Compare(byte value)
        {
            var before = Z80.Registers.A;
            var result = before - value;
            var byteResult = (byte)result;

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, byteResult.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, byteResult == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, ((before ^ result) & 0x10) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, before.HasBit(7) == value.HasBit(7) && before.HasBit(7) != byteResult.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, true);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.C, result < 0);
        }

        public byte Inc(byte value)
        {
            var result = (byte)(value + 1);

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, result.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, result == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, ((value ^ result) & 0xF) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, value == 0x7F);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, false);

            return result;
        }

        public ushort Inc(ushort value)
        {
            return (ushort)(value + 1);
        }

        public byte Dec(byte value)
        {
            var result = (byte)(value - 1);

            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.S, result.HasBit(7));
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.Z, result == 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.H, ((value ^ result) & 0xF) != 0);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.PV, value == 0x80);
            Z80.Registers.F = Z80.Registers.F.SetFlags(Flags.N, true);

            return result;
        }

        public ushort Dec(ushort value)
        {
            return (ushort)(value - 1);
        }

        public bool CheckFlags(int cc)
        {
            return cc switch
            {
                0b000 => !Z80.Registers.F.HasFlag(Registers.Flags.Z),
                0b001 => Z80.Registers.F.HasFlag(Registers.Flags.Z),
                0b010 => !Z80.Registers.F.HasFlag(Registers.Flags.C),
                0b011 => Z80.Registers.F.HasFlag(Registers.Flags.C),
                0b100 => !Z80.Registers.F.HasFlag(Registers.Flags.PV),
                0b101 => Z80.Registers.F.HasFlag(Registers.Flags.PV),
                0b110 => !Z80.Registers.F.HasFlag(Registers.Flags.S),
                0b111 => Z80.Registers.F.HasFlag(Registers.Flags.S),
                _ => throw new NotImplementedException()
            };
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
