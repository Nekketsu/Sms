using Sms.Memory;

namespace Sms.Cpu.Tests.Cli
{
    public class TestMapper : Mapper
    {
        private readonly byte[] data;
        private readonly ISet<ushort> dirtyAddresses;

        public TestMapper(int size)
        {
            data = new byte[size];
            dirtyAddresses = new HashSet<ushort>();
        }

        public override byte this[ushort address]
        {
            get => data[address];
            set => data[address] = value;
        }

        public override int Length => data.Length;

        public void Reset()
        {
            foreach (var address in dirtyAddresses)
            {
                data[address] = 0;
            }
        }
    }
}
