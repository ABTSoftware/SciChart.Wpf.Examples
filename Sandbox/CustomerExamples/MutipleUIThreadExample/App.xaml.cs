using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MutipleUIThreadExample
{
    public partial class App : Application
    {
        public Thread SecondUIThread { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (SecondUIThread != null) return;

            SecondUIThread = new Thread(() =>
            {
                var secondWindow = new SecondWindow();

                secondWindow.Left = (SystemParameters.PrimaryScreenWidth / 2) - (secondWindow.Width / 2) + 100;
                secondWindow.Top = (SystemParameters.PrimaryScreenHeight / 2) - (secondWindow.Height / 2) + 100;

                secondWindow.Closed += (s, t) => Dispatcher.CurrentDispatcher.InvokeShutdown();

                secondWindow.Show();
                secondWindow.Activate();

                Dispatcher.Run();
            });

            SecondUIThread.SetApartmentState(ApartmentState.STA);
            SecondUIThread.IsBackground = true;
            SecondUIThread.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

#if NETFRAMEWORK
            SecondUIThread?.Abort();
#endif
        }

        public static string CurrentThreadInfo => $"Thread #{Thread.CurrentThread.ManagedThreadId} [{Thread.CurrentThread.GetApartmentState()}]";
    }
}