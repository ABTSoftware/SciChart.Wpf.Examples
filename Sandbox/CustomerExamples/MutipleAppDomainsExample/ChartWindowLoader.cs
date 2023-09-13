using System;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting;

namespace MutipleAppDomainsExample
{
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
    public class ChartWindowLoader : MarshalByRefObject
    {
        private Window _window;

        public bool IsLoaded { get; private set; }

        private void StartThread()
        {
            VisualXcceleratorEngine.UseAutoShutdown = false;
            VisualXcceleratorEngine.RestartEngine();

            _window = new ChartWindow();
            _window.Show();
            _window.Activate();

            IsLoaded = true;
            Dispatcher.Run();
        }

        public void CreateWindow()
        {
            var thread = new Thread(StartThread);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public void CloseWindow()
        {
            var dispatcher = _window?.Dispatcher;

            if (dispatcher != null && !dispatcher.HasShutdownStarted)
            {
                try
                {
                    dispatcher.Invoke(_window.Close);
                }
                catch (TaskCanceledException)
                {
                    // Ignored
                }
            }
        }
    }
}
