using System;
using System.Linq;
using System.Threading;
using System.Windows;
using SciChart.Examples.Demo.Helpers.UsageTracking;
using Microsoft.Practices.Unity;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Controls.ExceptionView;
using SciChart.Wpf.UI.Bootstrap;
using SciChart.Wpf.UI.Bootstrap.Utility;
using SciChart.Wpf.UI.Reactive.Async;

namespace SciChart.Examples.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILogFacade Log = LogManagerFacade.GetLogger(typeof(App));
        private Bootstrapper _bootStrapper;
        private const string _devMode = "/devmode";

        public App()
        {
            Startup += Application_Startup;
            Exit += OnExit;
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            InitializeComponent();
        }

        private void App_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {            
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
            if (e.Args.Contains(_devMode))
            {
                DeveloperModManager.Manage.IsDeveloperMode = true;
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
                };

                _bootStrapper = new Bootstrapper(ServiceLocator.Container, new AttributedTypeDiscoveryService(new ExplicitAssemblyDiscovery(assembliesToSearch)));
                _bootStrapper.InitializeAsync().Then(() =>
                {
                    //Syncing usages 
                    var syncHelper = ServiceLocator.Container.Resolve<ISyncUsageHelper>();
                    syncHelper.LoadFromIsolatedStorage();

                    //Try sync with service
                    syncHelper.GetRatingsFromServer();
                    syncHelper.SendUsagesToServer();
                    syncHelper.SetUsageOnExamples();

                    _bootStrapper.OnInitComplete();
                }).Catch(ex =>
                {
                    Log.Error("Exception:\n\n{0}", ex);
                    MessageBox.Show("Exception occurred in SciChart.Examples.Demo Startup");
                });
            }
            catch (Exception caught)
            {
                Log.Error("Exception:\n\n{0}", caught);
                MessageBox.Show("Exception occurred in SciChart.Examples.Demo Startup");
            }
        }

        private void OnExit(object sender, ExitEventArgs exitEventArgs)
        {
            var usageCalc = ServiceLocator.Container.Resolve<IUsageCalculator>();
            usageCalc.UpdateUsage(null);

            var syncHelper = ServiceLocator.Container.Resolve<ISyncUsageHelper>();

            // Consider doing this a bit more often.
            syncHelper.SendUsagesToServer();
            syncHelper.WriteToIsolatedStorage();
        }
    }
}