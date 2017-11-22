using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using SciChart.Wpf.UI.Bootstrap;
using SciChart.Wpf.UI.Reactive.Async;

namespace SciChart.Sandbox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static TestSuiteBootstrapper _bootStrapper;

        public App()
        {
            Startup += App_Startup;
        }

        static void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Container = new UnityContainer();
                _bootStrapper = new TestSuiteBootstrapper(Container);
                _bootStrapper.Initialize();
            }
            catch (Exception caught)
            {
                MessageBox.Show("Exception occurred in Test Suite Startup");
            }
        }

        public static UnityContainer Container { get; set; }
    }

    public class TestSuiteBootstrapper : AbtBootstrapper
    {
        public TestSuiteBootstrapper(UnityContainer container) : base(container, null)
        {
        }

        public override void Initialize()
        {
            // base.Initialize();
            Container.RegisterInstance<ISchedulerContext>(new SchedulerContext(
                new SharedScheduler(TaskScheduler.FromCurrentSynchronizationContext(), DispatcherScheduler.Current),
                new SharedScheduler(TaskScheduler.Default, Scheduler.Default)));
        }
    }

}
