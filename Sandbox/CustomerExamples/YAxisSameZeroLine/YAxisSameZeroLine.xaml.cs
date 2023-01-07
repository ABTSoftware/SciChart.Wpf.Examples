using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Events;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace YAxisSameZeroLineExample
{
    // This test case is not functional / finished yet 

    public partial class YAxisSameZeroLine : Window
    {
        public YAxisSameZeroLine()
        {
            InitializeComponent();

            scs0.RenderableSeries.Add(new FastLineRenderableSeries() { DataSeries = GetData(5, 0.01)});
            scs1.RenderableSeries.Add(new FastLineRenderableSeries() { DataSeries = GetData(2, 0.2) });
        }

        private IDataSeries GetData(double amplitude, double damping)
        {
            var xyDataSeries = new XyDataSeries<double>();
            for (int i = 0; i < 1000; i++)
            {
                xyDataSeries.Append(i, Math.Sin(i*0.1)*amplitude);
                amplitude *= (1.0 - damping);
            }

            return xyDataSeries;
        }
    }

    public class YAxisHelper
    {
        internal static Dictionary<string, List<AxisInfo>> AxisByGroup = new Dictionary<string, List<AxisInfo>>();

        internal class AxisInfo : IDisposable
        {
            internal AxisBase Axis { get; set; }
            internal IDisposable Subscription { get; set; }

            public void Dispose()
            {
                Subscription?.Dispose();
            }
        }

        public static readonly DependencyProperty SyncZeroLineGroupProperty = DependencyProperty.RegisterAttached(
            "SyncZeroLineGroup", typeof(string), typeof(YAxisHelper),
            new PropertyMetadata(default(string), OnGroupChanged));

        public static void SetSyncZeroLineGroup(DependencyObject element, string value)
        {
            element.SetValue(SyncZeroLineGroupProperty, value);
        }

        public static string GetSyncZeroLineGroup(DependencyObject element)
        {
            return (string) element.GetValue(SyncZeroLineGroupProperty);
        }

        private static void OnGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var axis = d as AxisBase;
            if (axis == null)
            {
                throw new InvalidOperationException(
                    "VerticalChartGroupProperty can only be applied to AxisBase derived types");
            }

            string newGroupName = e.NewValue as string;
            string oldGroupName = e.OldValue as string;


            if (String.IsNullOrEmpty(newGroupName))
            {
                // Removing the axis from grouping
                DetachAxisFromGroup(axis, oldGroupName);
            }
            else
            {
                // Switching to a new group
                if (newGroupName != oldGroupName)
                {
                    if (!String.IsNullOrEmpty(oldGroupName))
                    {
                        // Remove the old group mapping
                        DetachAxisFromGroup(axis, oldGroupName);
                    }

                    // Add to group
                    AttachAxisToGroup(axis, newGroupName);
                }
            }
        }

        private static void DetachAxisFromGroup(AxisBase axis, string axisGroup)
        {
            // TODO: Get Axis from AxisByGroup, dispose the AxisInfo and remove from the dictionary
            throw new NotImplementedException();
        }

        private static void AttachAxisToGroup(AxisBase axis, string newGroupName)
        {
            lock (AxisByGroup)
            {
                if (!AxisByGroup.ContainsKey(newGroupName))
                {
                    AxisByGroup.Add(newGroupName, new List<AxisInfo>());
                }

                // Use Rx.FromEvent to get observable stream of VisibleRangeChanged events 
                IObservable<EventPattern<VisibleRangeChangedEventArgs>> observable =
                    Observable.FromEventPattern<VisibleRangeChangedEventArgs>(
                        h => axis.VisibleRangeChanged += h,
                        h => axis.VisibleRangeChanged -= h);

                // Subscribe to the observable and pass in parameters plus group name 
                var subscription = observable
                    .Subscribe(eventArgs => OnVisibleRangeChanged((AxisBase) eventArgs.Sender,
                        eventArgs.EventArgs.NewVisibleRange, newGroupName));

                // Add 
                AxisByGroup[newGroupName].Add(new AxisInfo() {Axis = axis, Subscription = subscription});
            }
        }

        private static void OnVisibleRangeChanged(AxisBase axis, IRange newVisibleRange, string groupName)
        {
            lock (AxisByGroup)
            {
                var otherAxis = AxisByGroup[groupName].Where(a => ReferenceEquals(a, axis) == false).ToList();

                // you have this axis
                // you have this axis VisibleRange
                // you have otherAxis list 
                // now do the calculation! 
                Debug.WriteLine($"Axis with name {axis.Tag} in group {groupName} reported VisibleRange= {newVisibleRange.Min}, {newVisibleRange.Max}");
            }
        }
    }
}
