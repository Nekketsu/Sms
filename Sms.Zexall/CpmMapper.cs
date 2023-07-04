using Sms.Memory;

namespace Sms.Zexall;

public class CpmMapper : Mapper
{
    private readonly byte[] data;
    //public ushort StartingAddress { get; }

    public override byte this[ushort address]
    {
        get => data[address];
        set
        {
            Console.WriteLine($"MEMORY[0x{address:x}] = 0x{value:x}");
            data[address] = value;
        }
    }

    public override int Length => data.Length;

    public CpmMapper(byte[] data/*, ushort startingAddres = 100*/)
    {
        //StartingAddress = startingAddres;

        //var padding = Enumerable.Repeat((byte)0, startingAddres);
        //this.data =  padding.Concat(data).ToArray();

        this.data = data;
    }
}
