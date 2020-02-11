using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;

namespace SciChart.Mvvm.Tutorial
{
    public interface IDataProvider
    {
        XyValues GetHistoricalData();
        void SubscribeUpdates(Action<XyValues> onDataUpdated);
    }

    public struct XyValues
    {
        public IList<double> XValues;
        public IList<double> YValues;
    }

    public class DummyDataProvider : IDataProvider
    {
        private readonly Random _random = new Random(Environment.TickCount);
        private double _last;

        private readonly double _bias = 0.01;
        private int _i;

        public XyValues GetHistoricalData()
        {
            const int initialDataCount = 1000;
            return GenerateRandomWalk(initialDataCount);
        }

        public void SubscribeUpdates(Action<XyValues> onDataUpdated)
        {
            // Don't do this in design mode! Or you will get an Out of memory exception in XAML Designer
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            // Subscribes to updates and pushes to view-models
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };

            timer.Tick += (s, e) =>
            {
                var xyValues = GenerateRandomWalk(10);
                onDataUpdated(xyValues);
            };

            timer.Start();
        }

        private XyValues GenerateRandomWalk(int count)
        {
            XyValues values = new XyValues
            {
                XValues = new List<double>(),
                YValues = new List<double>(),
            };

            for (int i = 0; i < count; i++)
            {
                var next = _last + (_random.NextDouble() - 0.5 + _bias);
                _last = next;

                values.XValues.Add(_i++);
                values.YValues.Add(next);
            }

            return values;
        }
    }
}