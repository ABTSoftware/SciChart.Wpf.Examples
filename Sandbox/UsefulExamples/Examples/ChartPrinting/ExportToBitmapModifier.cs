using System;
using System.Windows;
using Microsoft.Win32;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals;
using SciChart.Core;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Sandbox.Examples.ChartPrinting
{
    public class ExportToBitmapModifier : ChartModifierBase
    {
        public ExportToBitmapModifier()
        {
            ReceiveHandledEvents = true;
        }

        public override void OnModifierDoubleClick(ModifierMouseArgs e)
        {
            base.OnModifierDoubleClick(e);

            var surface = ParentSurface as SciChartSurface;

            if (surface != null)
            {
                var image = surface.ExportToBitmapSource();

                Clipboard.SetImage(image);

                MessageBox.Show("Success");
            }
        }

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);
            if (e.MouseButtons == MouseButtons.Right)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Png|*.png|Jpeg|*.jpeg|Bmp|*.bmp",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var surface = ParentSurface as SciChartSurface;
                    if(surface == null)
                        return;

                    var exportType = (ExportType)saveFileDialog.FilterIndex-1;

                    surface.ExportToFile(saveFileDialog.FileName,exportType, false);
                }
            }
        }
    }
}
