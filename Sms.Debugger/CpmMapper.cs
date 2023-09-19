using Sms.Memory;

namespace Sms.Debugger;

public class CpmMapper : Mapper
{
    private readonly byte[] data;

    public override byte this[ushort address]
    {
        get => data[address];
        set
        {
            data[address] = value;
        }
    }

    public override int Length => data.Length;

    public CpmMapper(byte[] data)
    {
        this.data = data;
    }
}
