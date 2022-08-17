﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;


namespace EliminatingFlickerExamplee
{
    /// <summary>
    /// Some users report flicker when the DirectX renderer is enabled.
    /// The [published workaround here](https://www.scichart.com/questions/question/flickering-chart) is implemented in this example to show you how to do it.
    ///
    /// Instructions for use
    ///
    ///  1.) Start the example. It will start by default in software mode
    ///  2.) use the GridSplitter to resize. Do you see flicker?
    ///  3.) Click 'Enable DirectX' now use the grid splitter again. Does it flicker?
    ///  4.) Finally, check the UseAlternativeFillSource and EnableForceWaitForGPU flags. This should eliminate flicker in DirectX which occurs on some PCs
    /// </summary>
    public partial class EliminatingFlicker : Window
    {
        private XyDataSeries<float> _xyDataSeries = new XyDataSeries<float>(1000) { AcceptsUnsortedData = true };
        private readonly Random _random = new Random();
        private int _index = 0;
        private float _yNext = 0f;

        public EliminatingFlicker()
        {
            InitializeComponent();

            for(int i = 0; i < 1000; i++)
                _xyDataSeries.Append(i, float.NaN);

            this.mountainSeries0.DataSeries = _xyDataSeries;
            this.mountainSeries1.DataSeries = _xyDataSeries;

            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _yNext = _yNext + ((float)_random.NextDouble() - 0.5f);

            using (_xyDataSeries.SuspendUpdates())
            {                
                if (_index == 999)
                {
                    // wrap around. Break the line with nan
                    _xyDataSeries.Append(_index+1, float.NaN);
                    _index = 0;
                }

                _xyDataSeries.Update(_index++, _yNext);
            }
        }

        private void UseAltFillSource_OnChecked(object sender, RoutedEventArgs e)
        {
            bool isChecked = ((CheckBox) sender).IsChecked == true;

            VisualXcceleratorEngine.UseAlternativeFillSource = isChecked;
        }

        private void EnableForceWait_OnChecked(object sender, RoutedEventArgs e)
        {
            bool isChecked = ((CheckBox)sender).IsChecked == true;

            VisualXcceleratorEngine.EnableForceWaitForGPU = isChecked;
        }
    }
}