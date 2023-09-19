using System.Diagnostics;

namespace Sms.Zexall
{
    public class SdscPorts : IPortMapping
    {
        public Dictionary<byte, Func<byte>> PortReaders => new Dictionary<byte, Func<byte>>();

        public Dictionary<byte, Action<byte>> PortWriters => new Dictionary<byte, Action<byte>>
        {
            [0x3E] = _ => { },
            [0xFC] = _ => { },
            [0xFD] = c => Debug.Write((char)c)
        };
    }
}
