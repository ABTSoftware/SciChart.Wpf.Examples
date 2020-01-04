using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SciChart.Charting.Visuals.RenderableSeries.Animations;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using Unity;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Controls.ExceptionView;
using SciChart.UI.Bootstrap;
using SciChart.UI.Bootstrap.Utility;
using SciChart.UI.Reactive.Async;
using SciChart.UI.Reactive.Traits;

namespace SciChart.Examples.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ILogFacade _log;
        private Bootstrapper _bootStrapper;
        private const string _devMode = "/DEVMODE";
        private const string _quickStart = "/UIAUTOMATIONTESTMODE";

        public App()
        {
            Startup += Application_Startup;
            Exit += OnExit; DispatcherUnhandledException += App_DispatcherUnhandledException;

            InitializeComponent();
        }
        
        public ILogFacade Log
        {
            get
            {
                if (UIAutomationTestMode) return new ConsoleLogger();
                _log = _log ?? LogManagerFacade.GetLogger(typeof(App));
                return _log;
            }
        }

        /// <summary>
        /// UIAutomationTestMode is enabled when /uiautomationTestMode is passed as command argument, and enables as fast as possible startup without animations, delays or unnecessary services. Used by UIAutomationTests
        /// </summary>
        public static bool UIAutomationTestMode { get; private set; }

        private void App_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {        
            Log.Error("An unhandled exception occurred. Showing view to user...", e.Exception);
                
            var exceptionView = new ExceptionView(e.Exception)
            {
                Owner = Application.Current != null ? Application.Current.MainWindow : null,
                WindowStartupLocation = Application.Current != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen,
            };
            exceptionView.ShowDialog();

            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Contains(_devMode, StringComparer.InvariantCultureIgnoreCase))
            {
                DeveloperModManager.Manage.IsDeveloperMode = true;
            }

            if (e.Args.Contains(_quickStart, StringComparer.InvariantCultureIgnoreCase))
            {
                // Used in automation testing, disable animations and delays in transitions 
                UIAutomationTestMode = true;
                SeriesAnimationBase.GlobalEnableAnimations = false;
            }

            try
            {
                Thread.CurrentThread.Name = "UI Thread";

                Log.Debug("--------------------------------------------------------------");
                Log.DebugFormat("SciChart.Examples.Demo: Session Started {0}",
                    DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"));
                Log.Debug("--------------------------------------------------------------");

                var assembliesToSearch = new[]
                {
                    typeof (MainWindowViewModel).Assembly,
                    typeof (AbtBootstrapper).Assembly, // SciChart.UI.Bootstrap
                    typeof (IViewModelTrait).Assembly, // SciChart.UI.Reactive 
                };

                _bootStrapper = new Bootstrapper(ServiceLocator.Container, new AttributedTypeDiscoveryService(new ExplicitAssemblyDiscovery(assembliesToSearch)));
                _bootStrapper.InitializeAsync().Then(() =>
                {
                    if (!UIAutomationTestMode)
                    {
                        // Do this on background thread
                        Task.Run(() =>
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

                    _bootStrapper.OnInitComplete();
                }).Catch(ex =>
                {
                    Log.Error("Exception:\n\n{0}", ex);
                    MessageBox.Show("Exception occurred in SciChart.Examples.Demo.\r\n. Please send log files located at %CurrentUser%\\AppData\\Local\\SciChart\\SciChart.Examples.Demo.log to support");
                });
            }
            catch (Exception caught)
            {
                Log.Error("Exception:\n\n{0}", caught);
                MessageBox.Show("Exception occurred in SciChart.Examples.Demo.\r\n. Please send log files located at %CurrentUser%\\AppData\\Local\\SciChart\\SciChart.Examples.Demo.log to support");
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