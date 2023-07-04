using System.Collections;

namespace Sms.Memory;

public abstract class Mapper : IMemory<ushort>
{
    public abstract byte this[ushort address] { get; set; }

    public abstract int Length { get; }
    public ushort ReadWord(ushort address)
    {
        var value1 = this[(ushort)(address + 1)];
        var value2 = this[address];

        var value = (ushort)((value1 << 8) | value2);

        return value;
    }

    public void WriteWord(ushort address, ushort value)
    {
        this[(ushort)(address + 1)] = (byte)(value >> 8);
        this[address] = (byte)value;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        for (var i = (ushort)0; i < Length; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}