using SciChart.Core.Extensions;
using SciChart.Drawing.Common;
using SciChart.Sandbox;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace RenderContextApi
{
    [TestCase("Using RenderContext API")]
    public partial class MainWindow : Window
    {
        private ITexture2D _texture;
        private int[] _pixels;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Read image pixels
            var path = Path.Combine(Environment.CurrentDirectory, @"..\..\Examples\UsingRenderContextAPI\Resources", "TestImage.jpg");
            BitmapSource bitmap = new BitmapImage(new Uri(path));
            _pixels = new int[bitmap.PixelWidth * bitmap.PixelHeight];
            bitmap.CopyPixels(_pixels, bitmap.PixelWidth * 4, 0);

            // Create a Texture
            using (var rc = RenderSurface.GetRenderContext())
            {
                // Texture creation is very consuming, so recreate it as seldom as possible
                if (_texture == null)
                {
                    _texture.SafeDispose();
                    _texture = rc.CreateTexture(bitmap.PixelWidth, bitmap.PixelHeight);
                }
            }

            // Draw a Texture
            RenderTexture();
        }

        private void RenderTexture()
        {
            var desiredWidth = (int)RenderSurface.ActualWidth;
            var desiredHeight = (int)RenderSurface.ActualHeight;

            // Redraw occurs on dispose
            using (var rc = RenderSurface.GetRenderContext())
            {
                // Copy pixel data
                _texture.SetData(_pixels);

                // Render
                rc.DrawTexture(_texture, new Rect(new Size(desiredWidth, desiredHeight)));
            }
        }

        private void OnDrawImageClick(object sender, RoutedEventArgs e)
        {
            RenderTexture();
        }

        private void OnDrawEllipseClick(object sender, RoutedEventArgs e)
        {
            var desiredWidth = (int)RenderSurface.ActualWidth;
            var desiredHeight = (int)RenderSurface.ActualHeight;
            var ellipseSize = 50;

            using (var rc = RenderSurface.GetRenderContext())
            using (var strokePen = rc.CreatePen(Colors.Yellow, true, 3))
            using (var fillBrush = rc.CreateBrush(Colors.Red))
            {
                rc.DrawEllipse(strokePen, fillBrush, new Point(desiredWidth / 2, desiredHeight / 2), ellipseSize, ellipseSize);
            }
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            RenderSurface.Clear();
        }
    }
}
