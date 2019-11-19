using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.Demo.Helpers;
using SciChart.UI.Bootstrap;
using SciChart.UI.Bootstrap.Utility;
using SciChart.UI.Reactive.Async;
using SciChart.UI.Reactive.Observability;
using Unity;

namespace SciChart.Examples.Demo
{
    internal class Bootstrapper : AbtBootstrapper
    {
        public event EventHandler<EventArgs> WhenInit;

        private readonly ILogFacade _logger = LogManagerFacade.GetLogger(typeof(Bootstrapper));

        public Bootstrapper(IUnityContainer container, IAttributedTypeDiscoveryService attributedTypeDiscovery) 
            : base(container, attributedTypeDiscovery)
        {
        }

        public Task InitializeAsync()
        {            
            try
            {
                _logger.InfoFormat("Initializing Async");
                Container.RegisterInstance(this);

                var sc = new SchedulerContext(
                    new SharedScheduler(TaskScheduler.FromCurrentSynchronizationContext(), DispatcherSchedulerEx.Current),
                    new SharedScheduler(TaskScheduler.Default, Scheduler.Default));

                Container.RegisterInstance<ISchedulerContext>(sc);            
                ObservableObjectBase.DispatcherSynchronizationContext = SynchronizationContext.Current;
            }
            catch (Exception e)
            {
                _logger.Error("An error occurred in the initialization block: ", e);
                throw;
            }

            return Task.Factory.StartNew(async () =>
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

                    //Direct3D11RenderSurface.InitEngineAsync().Then(r =>
                    //{
                    if (!App.UIAutomationTestMode)
                    {
                        // Force delay to show splash
                        await Task.Delay(3000);
                    }
                    vm.InitReady = true;
                    //});
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

            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}