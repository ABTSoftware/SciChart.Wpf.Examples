using System;
using System.Windows.Media;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    public class GanttItemViewModel : BaseViewModel
    {
        private bool _isCurrent;
        private double _completion;

        private Color _color;
        private DateTime _start, _end;

        public string Id { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
                OnPropertyChanged(nameof(Fill));
            }
        }

        public Brush Fill => new SolidColorBrush(Color);

        public DateTime Start
        {
            get => _start;
            set
            {
                _start = WeekDayStartDate(value);
                OnPropertyChanged(nameof(Start));
            }
        }

        public DateTime End
        {
            get => _end;
            set
            {
                _end = WeekDayEndDate(value);
                OnPropertyChanged(nameof(End));
            }
        }
        
        public double Completion
        {
            get => _completion;
            set
            {
                _completion = value;
                OnPropertyChanged(nameof(Completion));
            }
        }

        public bool IsCurrent
        {
            get => _isCurrent;
            set
            {
                _isCurrent = value;
                OnPropertyChanged(nameof(IsCurrent));
            }
        }
        
        public void CheckCompletion(DateTime currentDate)
        {
            IsCurrent = currentDate >= Start && currentDate <= End;

            if (IsCurrent)
            {
                Completion = (currentDate.Ticks - Start.Ticks) * 100 / (End.Ticks - Start.Ticks);
            }
            else
            {
                Completion = currentDate > End ? 100d : 0d;
            }
        }

        private DateTime WeekDayStartDate(DateTime startDate)
        {
            if (startDate.DayOfWeek == DayOfWeek.Saturday)
                return startDate.AddDays(2);
            
            if (startDate.DayOfWeek == DayOfWeek.Sunday)
                return startDate.AddDays(1);
            
            return startDate;
        }

        private DateTime WeekDayEndDate(DateTime endDate)
        {
            if (endDate.DayOfWeek == DayOfWeek.Saturday)
                return endDate.AddDays(-1);
            
            if (endDate.DayOfWeek == DayOfWeek.Sunday)
                return endDate.AddDays(-2);
            
            return endDate;
        }
    }
}