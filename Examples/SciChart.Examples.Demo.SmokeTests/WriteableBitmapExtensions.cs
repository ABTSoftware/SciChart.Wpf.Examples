using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlaUI.Core.Capturing;

namespace SciChart.Examples.Demo.SmokeTests
{
    public static class WriteableBitmapExtensions
    {
        /// <summary>
        /// Copies all the Pixels from the WriteableBitmap into a ARGB byte array.
        /// </summary>
        /// <param name="bmp">The WriteableBitmap.</param>
        /// <returns>The color buffer as byte ARGB values.</returns>
        public static byte[] ToByteArray(this WriteableBitmap bmp)
        {

            var width = bmp.PixelWidth;
            var height = bmp.PixelHeight;
            var stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);

            var bitmapData = new byte[height * stride];

            bmp.CopyPixels(bitmapData, stride, 0);
            return bitmapData;
        }
    }
}