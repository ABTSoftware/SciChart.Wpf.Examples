using System.Windows.Media;

namespace TimeLineControlExample
{
    public static class ColorExtensions
    {
        public static int ToArgb(this Color color)
        {
            return (int)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B));
        }
    }
}