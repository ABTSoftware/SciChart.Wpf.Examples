// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateRealTime3DUniformMeshChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting3D;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Data.Model;
using Viewport3D = SciChart.Charting3D.Viewport3D;

namespace SciChart.Examples.Examples.Charts3D.CreateRealtime3DCharts
{
    /// <summary>
    /// Interaction logic for CreateRealTime3DSurfaceMeshChart.xaml
    /// </summary>
    public partial class CreateRealTime3DSurfaceMeshChart : UserControl
    {
        private Timer _timer;
        private bool _isRunning;
        private readonly object _syncRoot = new object();

        public CreateRealTime3DSurfaceMeshChart()
        {
            InitializeComponent();
            lightModeComboBox.ItemsSource = Enum.GetValues(typeof(MainLightMode));
            lightModeComboBox.SelectedItem = MainLightMode.CameraForward;

            Loaded += (s, e) => OnStart();

            Unloaded += (s, e) => OnStop();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            OnStart();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            OnStop();
        }

        private void DataCombo_OnSelectionChanged(object sender, EventArgs e)
        {
            _isRunning = false;

            OnStart();
        }

        private void OnStart()
        {
            if (!IsLoaded || _isRunning) return;

            _isRunning = true;

            string whatData = (string)DataCombo.SelectedItem;
            int w = 0, h = 0;
            if (whatData == "3D Sinc 10 x 10") w = h = 10;
            if (whatData == "3D Sinc 25 x 25") w = h = 25;
            if (whatData == "3D Sinc 50 x 50") w = h = 50;
            if (whatData == "3D Sinc 100 x 100") w = h = 100;
            if (whatData == "3D Sinc 500 x 500") w = h = 500;
            if (whatData == "3D Sinc 1k x 1k") w = h = 1000;

            lock (_syncRoot)
            {
                _timer?.Stop();
            }

            var dataSeries = new UniformGridDataSeries3D<double>(w, h)
            {
                StartX = 0,
                StartZ = 0,
                StepX = 10 / (w - 1d),
                StepZ = 10 / (h - 1d),
                SeriesName = "Realtime Surface Mesh",
            };

            var frontBuffer = dataSeries.InternalArray;
            var backBuffer = new GridData<double>(w, h).InternalArray;

            int frames = 0;
            _timer = new Timer();
            _timer.Interval = 20;
            _timer.Elapsed += (s, arg) =>
            {
                lock (_syncRoot)
                {
                    double wc = w * 0.5, hc = h * 0.5;
                    double freq = Math.Sin(frames++ * 0.1) * 0.1 + 0.1;

                    // Each set of dataSeries[i,j] schedules a redraw when the next Render event fires. Therefore, we suspend updates so that we can update the chart once
                    // Data generation (Sin, Sqrt below) is expensive. We parallelize it by using Parallel.For for the outer loop
                    //  Equivalent of "for (int j = 0; j < h; j++)"
                    // This will result in more CPU usage, but we wish to demo the performance of the actual rendering, not the slowness of generating test data! :)
                    Parallel.For(0, h, i =>
                    {
                        var buf = frontBuffer;
                        for (int j = 0; j < w; j++)
                        {
                            // 3D Sinc function from http://www.mathworks.com/matlabcentral/cody/problems/1305-creation-of-2d-sinc-surface
                            // sin(pi*R*freq)/(pi*R*freq)
                            // R is distance from centre

                            double radius = Math.Sqrt((wc - i) * (wc - i) + (hc - j) * (hc - j));
                            var d = Math.PI * radius * freq;
                            var value = Math.Sin(d) / d;
                            buf[i][j] = double.IsNaN(value) ? 1.0 : value;
                        }
                    });

                    using (dataSeries.SuspendUpdates(false, true))
                    {
                        dataSeries.CopyFrom(frontBuffer);
                        var temp = backBuffer;
                        backBuffer = frontBuffer;
                        frontBuffer = temp;
                    }
                }
            };

            SurfaceMesh.DataSeries = dataSeries;
            _timer.Start();

            StartButton.IsChecked = true;
            PauseButton.IsChecked = false;
        }

        private void OnStop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _timer?.Stop();

                StartButton.IsChecked = false;
                PauseButton.IsChecked = true;
            }
        }

        private void ColorMapCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;

            // Valid combinations check. If palette is a LinearGradientBrush and we are in texture mode, switch to heightmap mode
            if (((BrushColorPalette)ColorMapCombo.SelectedItem).Brush is LinearGradientBrush &&
                (MeshPaletteModeCombo.SelectedItem.Equals(MeshPaletteMode.Textured) || MeshPaletteModeCombo.SelectedItem.Equals(MeshPaletteMode.TexturedSolidCells)))
            {
                MeshPaletteModeCombo.SelectedItem = MeshPaletteMode.HeightMapInterpolated;
            }
            // Valid combinations check. If palette is a TextureBrush and we are not in texture mode, switch to texture mode
            else if (((BrushColorPalette)ColorMapCombo.SelectedItem).Brush is VisualBrush &&
                (MeshPaletteModeCombo.SelectedItem.Equals(MeshPaletteMode.HeightMapInterpolated) || MeshPaletteModeCombo.SelectedItem.Equals(MeshPaletteMode.HeightMapSolidCells)))
            {
                MeshPaletteModeCombo.SelectedItem = MeshPaletteMode.Textured;
            }
        }

        private void MeshPaletteModeCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;

            // Valid combinations check. If mesh palette is a heightmap, then choose a gradient brush for the palette
            if ((MeshPaletteModeCombo.SelectedItem.Equals(MeshPaletteMode.HeightMapInterpolated) || MeshPaletteModeCombo.SelectedItem.Equals(MeshPaletteMode.HeightMapSolidCells))
                && ((BrushColorPalette)ColorMapCombo.SelectedItem).Brush is VisualBrush)
            {
                ColorMapCombo.SelectedIndex = 0;
            }
            // Valid combinations check. If mesh palette is textured, then choose a texture brush for the palette
            else if (MeshPaletteModeCombo.SelectedItem.Equals(MeshPaletteMode.Textured) ||
                MeshPaletteModeCombo.SelectedItem.Equals(MeshPaletteMode.TexturedSolidCells))
            {
                ColorMapCombo.SelectedIndex = ColorMapCombo.Items.Count - 1;
            }
        }

        private void LightSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SciChart3DSurface?.Viewport3D is Charting3D.Viewport3D viewport)
            {
                var vector = viewport.GetMainLightDirection();

                vector.x = (float)lightSliderX.Value;
                vector.y = (float)lightSliderY.Value;
                vector.z = (float)lightSliderZ.Value;

                viewport.SetMainLightDirection(vector);
            }
        }

        private void LightMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lightModeComboBox.SelectedItem is MainLightMode lightMode)
            {
                lightSlidersPanel.IsEnabled = lightMode == MainLightMode.GlobalSpace;

                if (SciChart3DSurface?.Viewport3D is Charting3D.Viewport3D viewport)
                {
                    viewport.SetMainLightMode(lightMode);

                    if (lightMode == MainLightMode.GlobalSpace)
                    {
                        var vector = viewport.GetMainLightDirection();

                        vector.x = (float)lightSliderX.Value;
                        vector.y = (float)lightSliderY.Value;
                        vector.z = (float)lightSliderZ.Value;

                        viewport.SetMainLightDirection(vector);
                    }
                }
            }
        }
    }
}
