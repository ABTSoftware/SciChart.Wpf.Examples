using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Examples.Demo.Helpers.UsageTracking
{
    public interface IUsageClientConfiguration
    {
        string Address { get; }
    }

    [ExportType(typeof(IUsageClientConfiguration), CreateAs.Singleton)]
    public class UsageClientConfiguration : IUsageClientConfiguration
    {
        #if !SILVERLIGHT
        public string Address
        {

            get { return Properties.Settings.Default.UsageServiceAddress; }

        }

        #else
        private string _address = "http://licensing.scichart.com";

        public string Address
        {
            get { return _address; }  
            set { _address = value; }
        }
        #endif
    } 
}
