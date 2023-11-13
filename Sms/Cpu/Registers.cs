using CommunityToolkit.Mvvm.ComponentModel;

namespace Sms.Cpu
{
    [ObservableObject]
    public partial class Registers
    {
        [ObservableProperty]
        private bool iFF1;
        [ObservableProperty]
        private bool iFF2;

        [ObservableProperty]
        private bool nMI;

        [ObservableProperty]
        private ushort pC;
        [ObservableProperty]
        private ushort sP;
        [ObservableProperty]
        private byte r;
        [ObservableProperty]
        private byte i;


        [ObservableProperty]
        private byte a;
        [ObservableProperty]
        private Flags f;
        [ObservableProperty]
        private byte b;
        [ObservableProperty]
        private byte c;
        [ObservableProperty]
        private byte d;
        [ObservableProperty]
        private byte e;
        [ObservableProperty]
        private byte h;
        [ObservableProperty]
        private byte l;

        [ObservableProperty]
        private byte aShadow;
        [ObservableProperty]
        private byte fShadow;
        [ObservableProperty]
        private byte bShadow;
        [ObservableProperty]
        private byte cShadow;
        [ObservableProperty]
        private byte dShadow;
        [ObservableProperty]
        private byte eShadow;
        [ObservableProperty]
        private byte hShadow;
        [ObservableProperty]
        private byte lShadow;

        [ObservableProperty]
        private ushort iX;
        [ObservableProperty]
        private ushort iY;

        public ushort AF
        {
            get => (ushort)((A << 8) + (byte)F);
            set
            {
                A = (byte)(value >> 8);
                F = (Flags)(byte)value;
            }
        }

        public ushort BC
        {
            get => (ushort)((B << 8) + C);
            set
            {
                B = (byte)(value >> 8);
                C = (byte)value;
            }
        }

        public ushort DE
        {
            get => (ushort)((D << 8) + E);
            set
            {
                D = (byte)(value >> 8);
                E = (byte)value;
            }
        }
        public ushort HL
        {
            get => (ushort)((H << 8) + L);
            set
            {
                H = (byte)(value >> 8);
                L = (byte)value;
            }
        }

        public ushort AFShadow
        {
            get => (ushort)((AShadow << 8) + FShadow);
            set
            {
                AShadow = (byte)(value >> 8);
                FShadow = (byte)value;
            }
        }

        public ushort BCShadow
        {
            get => (ushort)((BShadow << 8) + CShadow);
            set
            {
                BShadow = (byte)(value >> 8);
                CShadow = (byte)value;
            }
        }

        public ushort DEShadow
        {
            get => (ushort)((DShadow << 8) + EShadow);
            set
            {
                DShadow = (byte)(value >> 8);
                EShadow = (byte)value;
            }
        }

        public ushort HLShadow
        {
            get => (ushort)((HShadow << 8) + LShadow);
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
            /// <summary>
            /// Not used
            /// </summary>
            X1 = 8,
            /// <summary>
            /// Half Carry Flag
            /// </summary>
            H = 16,
            /// <summary>
            /// Not used
            /// </summary>
            X2 = 32,
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
