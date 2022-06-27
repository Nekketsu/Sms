namespace Sms.Cpu
{
    public class Registers
    {
        public bool IFF1 { get; set; }
        public bool IFF2 { get; set; }

        public bool NMI { get; set; }

        public ushort PC { get; set; }
        public ushort SP { get; set; }
        public byte R { get; set; }
        public byte I { get; set; }


        public byte A { get; set; }
        public Flags F { get; set; }
        public byte B { get; set; }
        public byte C { get; set; }
        public byte D { get; set; }
        public byte E { get; set; }
        public byte H { get; set; }
        public byte L { get; set; }

        public byte AShadow { get; set; }
        public byte FShadow { get; set; }
        public byte BShadow { get; set; }
        public byte CShadow { get; set; }
        public byte DShadow { get; set; }
        public byte EShadow { get; set; }
        public byte HShadow { get; set; }
        public byte LShadow { get; set; }

        public ushort IX { get; set; }
        public ushort IY { get; set; }

        public ushort AF
        {
            get => (ushort)(A << 8 + (byte)F);
            set
            {
                A = (byte)(value >> 8);
                F = (Flags)(byte)value;
            }
        }

        public ushort BC
        {
            get => (ushort)(B << 8 + C);
            set
            {
                B = (byte)(value >> 8);
                C = (byte)value;
            }
        }

        public ushort DE
        {
            get => (ushort)(D << 8 + E);
            set
            {
                D = (byte)(value >> 8);
                E = (byte)value;
            }
        }
        public ushort HL
        {
            get => (ushort)(H << 8 + L);
            set
            {
                H = (byte)(value >> 8);
                L = (byte)value;
            }
        }

        public ushort AFShadow
        {
            get => (ushort)(AShadow << 8 + FShadow);
            set
            {
                AShadow = (byte)(value >> 8);
                FShadow = (byte)value;
            }
        }

        public ushort BCShadow
        {
            get => (ushort)(BShadow << 8 + CShadow);
            set
            {
                BShadow = (byte)(value >> 8);
                CShadow = (byte)value;
            }
        }

        public ushort DEShadow
        {
            get => (ushort)(DShadow << 8 + EShadow);
            set
            {
                DShadow = (byte)(value >> 8);
                EShadow = (byte)value;
            }
        }
        public ushort HLShadow
        {
            get => (ushort)(HShadow << 8 + LShadow);
            set
            {
                HShadow = (byte)(value >> 8);
                LShadow = (byte)value;
            }
        }

        [Flags]
        public enum Flags : byte
        {
            /// <summary>
            /// Carry Flag
            /// </summary>
            C = 1,
            /// <summary>
            /// Substract Flag
            /// </summary>
            N = 2,
            /// <summary>
            /// Parity or Overflow Flag
            /// </summary>
            PV = 4,
            // Bit 3: Not used
            /// <summary>
            /// Half Carry Flag
            /// </summary>
            H = 16,
            // Bit 5: Not used
            /// <summary>
            /// Zero Flag
            /// </summary>
            Z = 64,
            /// <summary>
            /// Sign Flag
            /// </summary>
            S = 128
        }
    }
}
