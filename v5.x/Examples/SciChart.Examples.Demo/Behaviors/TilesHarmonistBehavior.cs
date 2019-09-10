using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Threading;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Behaviors
{
    public class TilesHarmonistBehavior : Behavior<WrapPanelCompatible>
    {
        private DispatcherTimer _timer;
        private int _counter;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2),
            };
            
            _timer.Tick += (_, __) =>
            {
                _counter++;
                foreach (ContentPresenter child in AssociatedObject.Children)
                {
                    var tile = (TileViewModel)child.Content;
                    if (_counter % tile.TransitionSeed == 0)
                    {
                        tile.ChangeState();
                    }
                }
            };

            _timer.Start();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }
    }
}