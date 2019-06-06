using System.ComponentModel;
using SciChart.Charting.Numerics.Calendars;

namespace DoubleScaleDiscontinuousAxis
{
    public class MainViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IDiscontinuousDateTimeCalendar _calendar = new WeekDaysAxisCalendar();

        public IDiscontinuousDateTimeCalendar AxisCalendar => _calendar;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
