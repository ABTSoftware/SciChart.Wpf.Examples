using System.Windows;

namespace PerformanceBenchmark
{
    public partial class App : Application
    {
        public static App Instance => Current as App;
    }
}
