using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SciChart.Charting;
using SciChart.Core.Extensions;
using SciChart.Drawing.Common;
using Path = System.IO.Path;

namespace UsingRenderContextApiExample
{
    public partial class MainWindow : Window
    {
        private ITexture2D _texture;
        private int[] _pixels;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Start VisualXcceleratorEngine, which is required for VisualXcceleratorRenderSurface
            VisualXcceleratorEngine.UseAutoShutdown = true;
            await VisualXcceleratorEngine.RestartEngineAsync();

            // Read image pixels
            var path = Path.Combine(Environment.CurrentDirectory, "Resources", "TestImage.jpg");
            BitmapSource bitmap = new BitmapImage(new Uri(path));
            _pixels = new int[bitmap.PixelWidth * bitmap.PixelHeight];
            bitmap.CopyPixels(_pixels, bitmap.PixelWidth * 4, 0);

            // Create a Texture
            using (var rc = RenderSurface.GetRenderContext() as IVxRenderContext)
            {
                rc.BeginFrame();

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
            using (var rc = RenderSurface.GetRenderContext() as IVxRenderContext)
            {
                rc.BeginFrame();

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

            using (var rc = RenderSurface.GetRenderContext() as IVxRenderContext)
            using (var strokePen = rc.CreatePen(Colors.Yellow, true, 3))
            using (var fillBrush = rc.CreateBrush(Colors.Red))
            {
                rc.BeginFrame();

                rc.DrawEllipse(strokePen, fillBrush, new Point(desiredWidth / 2, desiredHeight / 2), ellipseSize, ellipseSize);
            }
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            RenderSurface.Clear();
        }
    }
}
