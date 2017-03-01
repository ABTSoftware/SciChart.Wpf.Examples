using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using SciChart.Examples.Demo.Helpers;
using SciChart.Wpf.UI.Bootstrap;
using SciChart.Wpf.UI.Reactive.Async;
using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Examples.Demo
{
    internal class Bootstrapper : AbtBootstrapper
    {
        public event EventHandler<EventArgs> WhenInit;

        public Bootstrapper(
            IUnityContainer container, 
            IAttributedTypeDiscoveryService attributedTypeDiscovery) 
            : base(container, attributedTypeDiscovery)
        {
        }

        public Task InitializeAsync()
        {
            Container.RegisterInstance<Bootstrapper>(this);

            var sc = new SchedulerContext(
                new SharedScheduler(TaskScheduler.FromCurrentSynchronizationContext(), DispatcherScheduler.Current),
                new SharedScheduler(TaskScheduler.Default, Scheduler.Default));
            Container.RegisterInstance<ISchedulerContext>(sc);            
            ObservableObjectBase.DispatcherSynchronizationContext = SynchronizationContext.Current;

            return Task.Factory.StartNew(() =>
            {
                base.Initialize();

                // Do init async
                // Bootstrap example definitions
                Container.Resolve<IModule>().Initialize();
                var vm = ServiceLocator.Container.Resolve<IMainWindowViewModel>();

                // Bootstrap D3D to save time on startup            
                SciChart.Drawing.DirectX.Context.D3D10.Direct3D10RenderSurface.InitEngineAsync().Then(r =>
                {
                    vm.InitReady = true;
                });
            });
        }

        public void OnInitComplete()
        {
            var handler = WhenInit;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
