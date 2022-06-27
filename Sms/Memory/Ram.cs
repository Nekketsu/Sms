using System.Collections;

namespace Sms
{
    public class Ram : IMemory<ushort>
    {
        byte[] data;

        public int Length => data.Length;

        public byte this[ushort address]
        {
            get => data[address];
            set => data[address] = value;
        }

        public Ram(int length)
        {
            data = new byte[length];
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
