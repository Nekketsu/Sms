namespace Sms
{
    public class Screen
    {
        public int Width { get; }
        public int Height { get; }

        Color[,] data;

        public bool Disabled { get; set; }

        public Screen(int width, int height)
        {
            Width = width;
            Height = height;

            data = new Color[height, width];

            Disabled = false;
        }

        public Color this[int x, int y]
        {
            get => data[y, x];
            set => data[y, x] = value;
        }
    }
}
