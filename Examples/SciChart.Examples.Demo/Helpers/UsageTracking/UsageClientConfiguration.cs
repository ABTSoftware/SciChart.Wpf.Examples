// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// UsageClientConfiguration.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.UI.Bootstrap;

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
