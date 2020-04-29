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

            initStopwatch = Stopwatch.StartNew();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += (sender, eventArgs) => { timerTextBlock.Text = initStopwatch.Elapsed.ToString("ss\\.fff"); };
            timer.Start();
        }

        public void StopTimer()
        {
            initStopwatch.Stop();
            timer.Stop();
            timerTextBlock.Text = initStopwatch.Elapsed.ToString("ss\\.fff");
        }
    }
}
