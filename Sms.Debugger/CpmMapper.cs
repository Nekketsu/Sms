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

    public CpmMapper(byte[] data, int size, int offset = 0)
    {
        this.data = new byte[size];
        data.CopyTo(this.data, offset);

        this.data[0] = 0xd3;       /* OUT N, A */
        this.data[1] = 0x00;

        this.data[5] = 0xdb;       /* IN A, N */
        this.data[6] = 0x00;
        this.data[7] = 0xc9;       /* RET */
    }
}
