namespace Sms.Vdp
{
    public class Registers
    {
        /// <summary>
        /// Status register
        /// </summary>
        public byte Status { get; set; }
        /// <summary>
        /// Control registers
        /// </summary>
        private byte[] registers { get; }

        public byte this[int register]
        {
            get => registers[register];
            set => registers[register] = value;
        }

        public Registers()
        {
            registers = new byte[11];
        }
    }
}
