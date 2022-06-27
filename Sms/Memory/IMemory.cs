namespace Sms
{
    public interface IMemory<TAddress> : IEnumerable<byte>
    {
        int Length { get; }
        byte this[TAddress address] { get; }
    }
}
