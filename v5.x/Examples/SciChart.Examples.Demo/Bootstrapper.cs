using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.Unity;
using SciChart.Examples.Demo.Helpers;
using SciChart.Wpf.UI.Bootstrap;
using SciChart.Wpf.UI.Bootstrap.Utility;
using SciChart.Wpf.UI.Reactive.Async;
using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Examples.Demo
{
    internal class Bootstrapper : AbtBootstrapper
    {
        public event EventHandler<EventArgs> WhenInit;

        private ILogFacade _logger = LogManagerFacade.GetLogger(typeof(Bootstrapper));

        public Bootstrapper(
            IUnityContainer container, 
            IAttributedTypeDiscoveryService attributedTypeDiscovery) 
            : base(container, attributedTypeDiscovery)
        {
        }

        public Task InitializeAsync()
        {            
            try
            {
                _logger.InfoFormat("Initializing Async");
                Container.RegisterInstance<Bootstrapper>(this);

                var sc = new SchedulerContext(
                    new SharedScheduler(TaskScheduler.FromCurrentSynchronizationContext(), DispatcherScheduler.Current),
                    new SharedScheduler(TaskScheduler.Default, Scheduler.Default));
                Container.RegisterInstance<ISchedulerContext>(sc);            
                ObservableObjectBase.DispatcherSynchronizationContext = SynchronizationContext.Current;

            }
            catch (Exception e)
            {
                _logger.Error("An error occurred in the initialization block: ", e);
                throw;
            }

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    _logger.InfoFormat("... 1of4 base.Initialize()");
                    base.Initialize();

                    // Do init async
                    // Bootstrap example definitions
                    _logger.InfoFormat("... 2of4 IModule.Initializer()");
                    Container.Resolve<IModule>().Initialize();

                    _logger.InfoFormat("... 3of4 Resolve IMainWindowViewModel");
                    var vm = ServiceLocator.Container.Resolve<IMainWindowViewModel>();

                    // Bootstrap D3D to save time on startup     
                    _logger.InfoFormat("... 4of4 D3D11.Initialize()");
                    SciChart.Drawing.DirectX.Context.D3D11.Direct3D11RenderSurface.InitEngineAsync().Then(r =>
                    {
                        vm.InitReady = true;
                    });
                }
                catch (Exception e)
                {
                    _logger.Error("One or more errors occurred during initialization of the MainViewModel or DirectX11 engine", e);
                    throw;
                }
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
