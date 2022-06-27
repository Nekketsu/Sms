namespace Sms
{
    public class Joypads : IPortMapping
    {
        public byte PortA { get; set; }
        public byte PortB { get; set; }

        public Dictionary<byte, Func<byte>> PortReaders => new Dictionary<byte, Func<byte>>
        {
            [0xDC] = () => PortA,
            [0xC0] = () => PortA,
            [0xDD] = () => PortB,
            [0xC1] = () => PortB
        };

        public Dictionary<byte, Action<byte>> PortWriters => new Dictionary<byte, Action<byte>>();
    }
}
