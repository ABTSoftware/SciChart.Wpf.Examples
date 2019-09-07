using System.Windows.Media;

namespace Fifo100MillionPointsDemo.HelperClasses
{
    public class Colors
    {
        public static Color RandomColor()
        {
            return Color.FromRgb(Rand.NextByte(55, 255), 
                Rand.NextByte(55, 255), 
                Rand.NextByte(55, 255));
        }
    }
}