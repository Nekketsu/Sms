using System;
using System.Collections.Generic;

namespace Sms
{
    public interface IPortMapping
    {
        public Dictionary<byte, Func<byte>> PortReaders { get; }
        public Dictionary<byte, Action<byte>> PortWriters { get; }
    }
}