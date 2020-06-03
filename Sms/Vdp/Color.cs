namespace Sms
{
    public class Color
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static byte GetColorShade(byte val)
        {
            const byte step = 255 / (4 - 1); // 85

            return (byte)(val * step);
        }
    }
}
