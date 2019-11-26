using System;
using System.ComponentModel;
using System.Linq;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Numerics.Calendars;
using SciChart.Examples.ExternalDependencies.Data;

namespace DoubleScaleDiscontinuousDateTimeAxisExample
{
    public class MainViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IDataManager _dataManager = DataManager.Instance;
        private IDiscontinuousDateTimeCalendar _calendar = new WeekDaysAxisCalendar();
        private IOhlcDataSeries<DateTime, double> _dataSeries;

        public MainViewModel()
        {
            var instrument = _dataManager.AvailableInstruments.FirstOrDefault(x => x.Symbol == "INDU");
            var timeFrame = _dataManager.GetAvailableTimeFrames(instrument).First();

            var priceData = _dataManager.GetPriceData(instrument.Symbol, timeFrame);

            _dataSeries = new OhlcDataSeries<DateTime, double>();
            _dataSeries.Append(priceData.TimeData, priceData.OpenData, priceData.HighData, priceData.LowData, priceData.CloseData);
        }

        public IDiscontinuousDateTimeCalendar AxisCalendar => _calendar;

        public IDataSeries DataSeries => _dataSeries;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
