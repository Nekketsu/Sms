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
    }
}
