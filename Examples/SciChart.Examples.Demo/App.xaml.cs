using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting;
using SciChart.Charting.Visuals.RenderableSeries.Animations;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Controls.ExceptionView;
using SciChart.UI.Bootstrap;
using SciChart.UI.Bootstrap.Utility;
using SciChart.UI.Reactive.Traits;
using Unity;

namespace SciChart.Examples.Demo
{
    public partial class App : Application
    {
        private static ILogFacade _log;
        private Bootstrapper _bootstrapper;

        private const string DevMode = "/DEVMODE";
        private const string QuickStart = "/UIAUTOMATIONTESTMODE";
        private const int SplashDelay = 3000;

        public App()
        {
            Startup += OnStartup;
            Exit += OnExit;
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            InitializeComponent();
        }

        public static ILogFacade Log
        {
            get
            {
                if (UIAutomationTestMode)
                {
                    _log ??= new ConsoleLogger();
                }
                else
                {
                    _log ??= LogManagerFacade.GetLogger(typeof(App));
                }
                return _log;
            }
        }

        /// <summary>
        /// UIAutomationTestMode is enabled when /uiautomationTestMode is passed as command argument, and enables as fast as possible startup without animations, delays or unnecessary services. Used by UIAutomationTests
        /// </summary>
        public static bool UIAutomationTestMode { get; private set; }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error("An unhandled exception occurred. Showing view to user...", e.Exception);

            var mainWindowViewModel = ServiceLocator.Container.Resolve<IMainWindowViewModel>();

            var exampleName = mainWindowViewModel?.SelectedExample?.Title ?? "No Example";
            var message = $"Selected Example: {exampleName}";
            var exceptionView = new ExceptionView(e.Exception, message)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            exceptionView.ShowDialog();

            e.Handled = true;
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Contains(DevMode, StringComparer.InvariantCultureIgnoreCase))
            {
                DeveloperModManager.Manage.IsDeveloperMode = true;
            }

            if (e.Args.Contains(QuickStart, StringComparer.InvariantCultureIgnoreCase))
            {
                // Used in automation testing, disable animations and delays in transitions 
                UIAutomationTestMode = true;
                SeriesAnimationBase.GlobalEnableAnimations = false;
            }

            try
            {
                Thread.CurrentThread.Name = "UI Thread";

                Log.Debug("--------------------------------------------------------------");
                Log.DebugFormat("SciChart.Examples.Demo: Session Started {0:dd MMM yyyy HH:mm:ss}", DateTime.Now);
                Log.Debug("--------------------------------------------------------------");

                var assembliesToSearch = new[]
                {
                    typeof(MainWindowViewModel).Assembly,
                    typeof(AbtBootstrapper).Assembly, // SciChart.UI.Bootstrap
                    typeof(IViewModelTrait).Assembly, // SciChart.UI.Reactive 
                };

                var assemblyDiscovery = new ExplicitAssemblyDiscovery(assembliesToSearch);
                var typeDiscoveryService = new AttributedTypeDiscoveryService(assemblyDiscovery);

                _bootstrapper = new Bootstrapper(ServiceLocator.Container, typeDiscoveryService);

                await Task.WhenAll(_bootstrapper.InitializeAsync(), Task.Delay(SplashDelay));

                if (UIAutomationTestMode)
                {
                    VisualXcceleratorEngine.EnableForceWaitForGPU = true;
                }
                else
                {
                    // Do this on background thread
                    _ = Task.Run(() =>
                    {
                        //Syncing usages 
                        var syncHelper = ServiceLocator.Container.Resolve<ISyncUsageHelper>();
                        syncHelper.LoadFromIsolatedStorage();

                        //Try sync with service
                        syncHelper.GetRatingsFromServer();
                        syncHelper.SendUsagesToServer();
                        syncHelper.SetUsageOnExamples();
                    });
                }

                _bootstrapper.OnInitComplete();
            }
            catch (Exception ex)
            {
                Log.Error("Exception:\n\n{0}", ex);

                MessageBox.Show("Please send log files located at %CurrentUser%\\AppData\\Local\\SciChart\\SciChart.Examples.Demo.log to support", "SciChart.Examples.Demo Exception");
            }
        }

        private void OnExit(object sender, ExitEventArgs exitEventArgs)
        {
            if (!UIAutomationTestMode)
            {
                var usageCalc = ServiceLocator.Container.Resolve<IUsageCalculator>();

                usageCalc.UpdateUsage(null);

                var syncHelper = ServiceLocator.Container.Resolve<ISyncUsageHelper>();

                // Consider doing this a bit more often.
                // And not on close as it generates server errors.
                //syncHelper.SendUsagesToServer();
                syncHelper.WriteToIsolatedStorage();
            }
        }
    }
}