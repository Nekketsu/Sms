using System.Numerics;
using static Sms.Cpu.Registers;

namespace Sms
{
    public static class BitExtensions
    {
        public static bool HasBit(this byte b, int bitPosition)
        {
            return (b & (1 << bitPosition)) != 0;
        }

        public static byte SetBit(this byte b, int bitPosition, bool on = true)
        {
            var mask = 1 << bitPosition;

            return on ? (byte)(b | mask) : (byte)(b & ~mask);
        }

        public static byte ResetBit(this byte b, int bitPosition)
        {
            var mask = 1 << bitPosition;

            return (byte)(b & ~mask);
        }
        public static bool HasBit(this ushort b, int bitPosition)
        {
            return (b & (1 << bitPosition)) != 0;
        }

        public static byte SetBit(this ushort b, int bitPosition, bool on = true)
        {
            var mask = 1 << bitPosition;

            return on ? (byte)(b | mask) : (byte)(b & ~mask);
        }

        public static byte ResetBit(this ushort b, int bitPosition)
        {
            var mask = 1 << bitPosition;

            return (byte)(b & ~mask);
        }

        public static Flags SetFlags(this Flags value, Flags flags, bool on)
        {
            return on ? value |= flags : value &= ~flags;
        }

        public static bool HasEvenParity(this byte value)
        {
            var count = 0;

            for (var i = 0; i < sizeof(byte) * 8; i++)
            {
                var mask = 1 << i;
                count += ((value & mask) != 0) ? 1 : 0;
            }

            return count % 2 == 0;
        }

        public static bool HasEvenParity(this ushort value)
        {
            var count = 0;

            for (var i = 0; i < sizeof(ushort) * 8; i++)
            {
                var mask = 1 << i;
                count += ((value & mask) != 0) ? 1 : 0;
            }

            return count % 2 == 0;
        }
    }
}
