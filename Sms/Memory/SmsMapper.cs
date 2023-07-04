namespace Sms.Memory
{
    public class SmsMapper : Mapper
    {
        public override int Length => 0x10000;

        private readonly Cartridge cartridge;

        private int firstBankPage;
        private int secondBankPage;
        private int thirdBankPage;

        private int currentRam;
        private Ram[] ramBanks;
        private byte[] internalMemory;

        public SmsMapper(Cartridge cartridge)
        {
            this.cartridge = cartridge;

            internalMemory = cartridge.Take(3 * MasterSystem.PageLength).ToArray();

            firstBankPage = 0;
            secondBankPage = 1;
            thirdBankPage = 2;

            currentRam = -1;
            ramBanks = Enumerable.Repeat(new Ram(0x4000), 2).ToArray();
        }


        public override byte this[ushort address]
        {
            get
            {
                var addr = address;
                // Read from mirror 0xDFFC-0XDFFF not the memory map registers
                if (addr >= 0xFFFC)
                {
                    addr -= 0x2000;
                }

                // The fixed memory address in slot 1
                else if (!cartridge.IsCodeMasters && addr < 0x400)
                {
                    return internalMemory[addr];
                }
                // Slot 1
                else if (addr < 0x4000)
                {
                    // Convert address to correct page address
                    var bankAddress = (ushort)(addr + (firstBankPage * MasterSystem.PageLength));
                    return cartridge[bankAddress];
                }
                // Slot 2
                else if (addr < 0x8000)
                {
                    // Convert address to correct page address
                    var bankAddress = (ushort)(addr + secondBankPage * MasterSystem.PageLength);
                    // Remove offset
                    bankAddress -= 0x4000;
                    return cartridge[bankAddress];
                }
                // Slot 3
                else if (addr < 0xC000)
                {
                    // Is RAM banking mapped in this slot?
                    if (currentRam > -1)
                    {
                        return ramBanks[currentRam][(ushort)(addr - 0x8000)]; // 0x8000 offset
                    }
                    else
                    {
                        // Convert address to correct page address
                        var bankAddress = (ushort)(addr + thirdBankPage * MasterSystem.PageLength);
                        return cartridge[bankAddress];
                    }
                }

                return internalMemory[addr];
            }
            set
            {
                // Handle the codemasters memory mapping
                if (cartridge.IsCodeMasters)
                {
                    if (address == 0x0 || address == 0x4000 || address == 0x8000)
                    {
                        DoMemPageCm(address, value);
                    }
                }

                // Cant write to rom
                if (address < 0x8000)
                {
                    return;
                }

                // Allow writing here if a ram bank is mapped into slot 3
                else if (address < 0xC000)
                {
                    var controlMap = this[0xFFFC];
                    if (currentRam > -1)
                    {
                        ramBanks[currentRam][(ushort)(address - 0x8000)] = value;
                    }
                    else
                    {
                        // This is ROM so lets return
                        return;
                    }
                }

                // It looks ok to write to memory
                this[address] = value;

                // Handle standard memory paging
                if (address >= 0xFFFC)
                {
                    if (!cartridge.IsCodeMasters)
                    {
                        DoMemPage(address, value);
                    }
                }

                // Handle mirroring
                if (address >= 0xC000 && address < 0xDFFC)
                {
                    internalMemory[(ushort)(address + 0x2000)] = value;
                }
                if (address >= 0xE000)
                {
                    internalMemory[(ushort)(address - 0x2000)] = value;
                }
            }
        }

        private void DoMemPage(ushort address, byte data)
        {
            var page = (byte)(cartridge.IsOneMegCartridge ? data & 0x3F : data & 0x1F);

            switch (address)
            {
                case 0xFFC:
                    // Check for slot 2 RAM banking
                    if (data.HasBit(3))
                    {
                        // Which of the two RAM banks are we swapping int?
                        currentRam = data.HasBit(2) ? 1 : 0;
                    }
                    else
                    {
                        currentRam = -1;
                    }
                    break;

                case 0xFFFD:
                    firstBankPage = page;
                    break;
                case 0xFFFE:
                    secondBankPage = page;
                    break;
                case 0xFFFF:
                    // Only allow ROM banking in slot 2 if RAM is not mapped there!
                    if (!this[0xFFFC].HasBit(3))
                    {
                        thirdBankPage = page;
                    }
                    break;
            }
        }

        private void DoMemPageCm(ushort address, byte data)
        {
            byte page = (byte)(cartridge.IsOneMegCartridge ? data & 0x3F : data & 0x1F);
            switch (address)
            {
                case 0x0:
                    firstBankPage = page;
                    break;
                case 0x4000:
                    secondBankPage = page;
                    break;
                case 0x8000:
                    thirdBankPage = page;
                    break;
            }
        }
    }
}
