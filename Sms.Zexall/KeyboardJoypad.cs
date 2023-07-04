namespace Sms.Zexall
{
    public class KeyboardJoyPad : IPortMapping
    {
        [Flags]
        enum PortDc : byte
        {
            PortAUp = 1,
            PortADown = 2,
            PortALeft = 4,
            PortARight = 8,
            PortATl = 16,
            PortATr = 32,
            PortBUp = 64,
            PortBDown = 128
        }

        [Flags]
        enum PortDd : byte
        {
            PortBLeft = 1,
            PortBRight = 2,
            PortBTl = 4,
            PortBTr = 8,
            Reset = 16,
            CartridgeSlot = 32,
            PortATh = 64,
            PortBTh = 128
        }

        public byte PortA { get; private set; } = 0b11111111;
        public byte PortB { get; private set; } = 0b11111111;

        public void Update()
        {
            PortA = 0b11111111;
            PortB = 0b11111111;

            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                MapKey(key);
            }
        }

        private void MapKey(ConsoleKeyInfo key)
        {
            var keyboardMap = new Dictionary<ConsoleKey, PortDc>
            {
                [ConsoleKey.W] = PortDc.PortAUp,
                [ConsoleKey.S] = PortDc.PortADown,
                [ConsoleKey.A] = PortDc.PortALeft,
                [ConsoleKey.D] = PortDc.PortARight,
                [ConsoleKey.U] = PortDc.PortATl,
                [ConsoleKey.I] = PortDc.PortATr
            };

            if (keyboardMap.TryGetValue(key.Key, out var portDc))
            {
                PortA = (byte)((PortDc)PortA & ~portDc);
            }
        }

        public Dictionary<byte, Func<byte>> PortReaders => new Dictionary<byte, Func<byte>>
        {
            [0xDC] = () => PortA,
            [0xC0] = () => PortA,
            [0xDD] = () => PortB,
            [0xC1] = () => PortB
        };

        public Dictionary<byte, Action<byte>> PortWriters => new Dictionary<byte, Action<byte>>();
    }
}
