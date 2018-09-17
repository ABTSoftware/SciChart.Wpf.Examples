using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SciChart.Drawing.Extensions;

namespace SciChart.Sandbox.Examples.ChartPrinting
{
    [TestCase("Chart Printing Multi Pane Charts")]
    public partial class ChartPrintingMultiPaneCharts : UserControl
    {
        public ChartPrintingMultiPaneCharts()
        {
            InitializeComponent();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(ChartGroup, "Test");
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Png Image (.png)|*.png";
            dialog.AddExtension = true;
            if (dialog.ShowDialog() == true)
            {
                var writeableBitmap = ChartGroup.RenderToBitmap((int)ChartGroup.ActualWidth, (int)ChartGroup.ActualHeight);
                using (FileStream stream5 = new FileStream(dialog.FileName, FileMode.Create))
                {
                    PngBitmapEncoder encoder5 = new PngBitmapEncoder();
                    encoder5.Frames.Add(BitmapFrame.Create(writeableBitmap));
                    encoder5.Save(stream5);
                }

                Process.Start(dialog.FileName);
            }
        }
    }
}
