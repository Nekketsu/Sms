namespace Sms.Cpu
{
    public class Ports : IPortMapping
    {
        public Dictionary<byte, Func<byte>> PortReaders => new Dictionary<byte, Func<byte>>();

        public Dictionary<byte, Action<byte>> PortWriters => new Dictionary<byte, Action<byte>>();

        public void MapPorts(IPortMapping portMapping)
        {
            foreach (var portReader in portMapping.PortReaders)
            {
                PortReaders.Add(portReader.Key, portReader.Value);
            }

            foreach (var portWriter in portMapping.PortWriters)
            {
                PortWriters.Add(portWriter.Key, portWriter.Value);
            }
        }

        public byte this[byte port]
        {
            get => PortReaders[port]();
            set => PortWriters[port](value);
        }
    }
}
