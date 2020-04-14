using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for LoadingContent.xaml
    /// </summary>
    public partial class LoadingContent : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        private Stopwatch initStopwatch;
        public LoadingContent()
        {
            InitializeComponent();

            timerTextBlock.Loaded += (s, e) =>
            {
                initStopwatch = Stopwatch.StartNew();
                timer.Interval = TimeSpan.FromMilliseconds(10);
                timer.Tick += (sender, eventArgs) => { timerTextBlock.Text = initStopwatch.Elapsed.ToString("ss\\.fff"); };
                timer.Start();
            };
        }

        public void StopTimer()
        {
            timer.Stop();
        }
    }
}
