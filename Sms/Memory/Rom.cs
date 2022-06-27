using System.Collections;

namespace Sms
{
    public class Rom : IMemory<ushort>
    {
        byte[] data;

        public int Length => data.Length;
        public byte this[ushort address] => data[address];

        public Rom(byte[] data)
        {
            this.data = data;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return (IEnumerator<byte>)data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
