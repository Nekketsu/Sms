using System;
using System.Collections.Generic;

namespace Sms.Debugger;

public class TestVdp : IPortMapping
{
    public Dictionary<byte, Func<byte>> PortReaders => new Dictionary<byte, Func<byte>>
    {
        [0xBF] = () => 0b10000000
    };

    public Dictionary<byte, Action<byte>> PortWriters => new Dictionary<byte, Action<byte>>();
}
