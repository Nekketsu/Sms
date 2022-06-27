using Sms.Memory;
using System.Collections;

namespace Sms
{
    public class Cartridge : IMemory<uint>
    {
        byte[] data;
        public bool IsOneMegCartridge { get; }
        public bool IsCodeMasters { get; }
        public int Length => data.Length;
        public Mapper Mapper { get; }

        public byte this[uint address] => data[address];

        public Cartridge(byte[] data)
        {
            this.data = data;

            IsOneMegCartridge = Length > 0x80000;
            IsCodeMasters = GetIsCodeMasters();

            Mapper = new Mapper(this);
        }

        public static Cartridge FromFile(string fileName)
        {
            const int headerLength = 512;

            var file = File.ReadAllBytes(fileName);

            var data = (file.Length % MasterSystem.PageLength == headerLength)
                ? file.Skip(headerLength).ToArray() // Has header
                : file;

            return new Cartridge(data);
        }

        private bool GetIsCodeMasters()
        {
            var checksum = (ushort)(this[0x7fe7] << 8);
            checksum |= this[0x7fe6];

            if (checksum == 0x0)
            {
                return false;
            }

            var compute = 0xffff - checksum + 1;

            var answer = (ushort)(this[0x7fe9] << 8);
            answer |= this[0x7fe9];

            return compute == answer;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return (IEnumerator<byte>)(data.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
