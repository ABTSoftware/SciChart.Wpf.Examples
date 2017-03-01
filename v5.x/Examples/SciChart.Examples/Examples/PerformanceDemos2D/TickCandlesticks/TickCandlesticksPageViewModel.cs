using System;
using System.Linq;
using System.Threading.Tasks;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.PerformanceDemo.Common.DataGenerators;
using SciChart.PerformanceDemo.Wpf;
using SciChart.PerformanceDemo.Wpf.TestPages;

namespace Abt.Controls.SciChart.PerformanceDemo.TestPages.TickCandlesticks
{
    public class TickCandlesticksPageViewModel : PageViewModel
    {
        private readonly string _exampleTitle;
        private readonly string _exampleSubtitle;

        private OhlcDataSeries<DateTime, double> _priceSeries;
        private XyDataSeries<DateTime, double> _maLowSeries;
        private XyDataSeries<DateTime, double> _maHighSeries;
        private MarketDataService _marketDataService;

        public TickCandlesticksPageViewModel(MainViewModel parent) : base(parent)
        {
            _exampleTitle = "1 Million Candlesticks";
            _exampleSubtitle = "Realtime updating of a chart with 1M OHLC";

            RunExampleCommand = new ActionCommand(OnRunExample);

            // Market data service simulates live ticks. We want to load the chart with 150 historical bars
            // then later do real-time ticking as new data comes in
            _marketDataService = new MarketDataService(new DateTime(2000, 08, 01, 12, 00, 00), 5, 20);
        }

        public ActionCommand RunExampleCommand { get; private set; }

        public OhlcDataSeries<DateTime, double> PriceSeries
        {
            get { return _priceSeries; }
            set
            {
                if (_priceSeries == value) return;
                _priceSeries = value;
                OnPropertyChanged("PriceSeries");
            }
        }

        public XyDataSeries<DateTime, double> MaLowSeries
        {
            get { return _maLowSeries; }
            set
            {
                if (_maLowSeries == value) return;
                _maLowSeries = value;
                OnPropertyChanged("MaLowSeries");
            }
        }

        public XyDataSeries<DateTime, double> MaHighSeries
        {
            get { return _maHighSeries; }
            set
            {
                if (_maHighSeries == value) return;
                _maHighSeries = value;
                OnPropertyChanged("MaHighSeries");
            }
        }


        public override string ExampleTitle
        {
            get { return _exampleTitle; }
        }

        public override string ExampleSubtitle
        {
            get { return _exampleSubtitle; }
        }

        public override void OnPageExit()
        {
            base.OnPageExit();

            Messages.Clear();
            OpacityParamsForInstructions = null;
        }

        private void OnRunExample()
        {
            Messages.Clear();
            OpacityParamsForInstructions = null;

            Task.Factory.StartNew(() =>
                {
                    // Add DataSeries for the candlestick and SMA series
                    var ds0 = new OhlcDataSeries<DateTime, double>() {SeriesName = "$SPX500"};
                    var ds1 = new XyDataSeries<DateTime, double>() {SeriesName = "MA 200"};
                    var ds2 = new XyDataSeries<DateTime, double>() {SeriesName = "MA 50"};

                    Messages.Add("Generating 1M candlesticks ...");
                    // Append historical bars to data series
                    var prices = _marketDataService.GetHistoricalData(1000000);

                    Messages.Add("Appending Data to chart ...");
                    ds0.Append(
                        prices.Select(x => x.DateTime),
                        prices.Select(x => x.Open),
                        prices.Select(x => x.High),
                        prices.Select(x => x.Low),
                        prices.Select(x => x.Close));
                    //ds1.Append(prices.Select(x => x.DateTime), prices.Select(y => _sma50.Push(y.Close).Current));

                    PriceSeries = ds0;
                    Messages.Add("Done!");

                    ViewportManager.ZoomExtents();
                });
        }
    }
}