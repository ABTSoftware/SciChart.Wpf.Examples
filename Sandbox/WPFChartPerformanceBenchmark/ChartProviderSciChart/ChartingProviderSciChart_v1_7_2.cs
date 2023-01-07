using System;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Numerics;
using ChartProviders.Common;

namespace ChartProviderSciChart_v1_7_2
{
    public class ChartingProviderSciChart_v1_7_2 : IChartingProvider
    {
        static ChartingProviderSciChart_v1_7_2()
        {
            SciChartSurface.SetLicenseKey(@"<LicenseContract>
                                              <Customer>ABT</Customer>
                                              <OrderId>Test</OrderId>
                                              <LicenseCount>1</LicenseCount>
                                              <IsTrialLicense>false</IsTrialLicense>
                                              <SupportExpires>09/07/2013 00:00:00</SupportExpires>
                                              <KeyCode>lwAAAAEAAACd79h9vqvOATkAQ3VzdG9tZXI9QUJUO09yZGVySWQ9VGVzdDtTdWJzY3JpcHRpb25WYWxpZFRvPTA3LVNlcC0yMDEzY4hf1A2pl6mzuJHCr296894GLSQVToXjaeEKo70ufpYOMj/1gZYU/wvNytpDJd7m</KeyCode>
                                            </LicenseContract>");
        }

        public ISpeedTest ScatterPointsSpeedTest()
        {
            return new SciChartScatterSeries();
        }

        public ISpeedTest FifoLineSpeedTest()
        {
            return new FifoLineSeriesSpeedTest();
        }

        public ISpeedTest LineAppendSpeedTest()
        {
            return new LineSeriesAppendingSpeedTest();
        }

        public ISpeedTest LoadNxNRefreshTest()
        {
            return new Load500x500SeriesRefreshTest();
        }

        public string Name
        {
            get { return "SciChart v1.7.2"; }
        }
    }
}
