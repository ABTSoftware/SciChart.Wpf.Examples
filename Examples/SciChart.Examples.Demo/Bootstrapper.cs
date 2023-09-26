using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using SciChart.Charting;
using SciChart.Charting3D;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Search;
using SciChart.Examples.Demo.ViewModels;
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
            catch (Exception ex)
            {
                _logger.Error("An error occurred in the initialization block: ", ex);
                throw;
            }

            return Task.Run(async () =>
            {
                try
                {
                    var loadLibsTask = SciChart2D3DInitializer.LoadLibrariesAndLicenseAsync();

                    _logger.InfoFormat("... 1 of 5 base.Initialize()");

                    base.Initialize();

                    _logger.InfoFormat("... 2 of 5 IModule.Initialize()");

                    var module = Container.Resolve<IModule>();

                    module.Initialize();

                    _logger.InfoFormat("... 3 of 5 Create Search Index");

                    CreateInvertedIndex.CreateIndex(module.Examples);
                    CreateInvertedIndex.CreateIndexForCode(module.Examples);

                    await loadLibsTask;

                    _logger.InfoFormat("... 4 of 5 Initialize Visual Xccelerator Engine");

                    try
                    {
                        VisualXcceleratorEngine.UseAutoShutdown = false;
                        VisualXcceleratorEngine.RestartEngine();
                    }
                    catch
                    {
                        // Suppress Vx init errors. All rendering will occur with a fallback
                    }

                    _logger.InfoFormat("... 5 of 5 Resolve View Models");

                    var mainViewModel = ServiceLocator.Container.Resolve<IMainWindowViewModel>();
                    var settingsViewModel = ServiceLocator.Container.Resolve<ISettingsViewModel>();
                    var exampleViewModel = ServiceLocator.Container.Resolve<IExampleViewModel>();

                    mainViewModel.SearchBoxEnabled = true;
                    mainViewModel.InitReady = true;

                    settingsViewModel.InitReady = true;
                    exampleViewModel.InitReady = true;
                }
                catch (Exception ex)
                {
                    _logger.Error("One or more errors occurred while initializing the MainViewModel or creating the search index: ", ex);
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