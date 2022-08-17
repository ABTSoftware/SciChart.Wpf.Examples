using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    public class GanttTextLabelProvider : IPointLabelProvider
    {
        private readonly GanttItemViewModel _item;

        public GanttTextLabelProvider(GanttItemViewModel item)
        {
            _item = item;
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }

        public string GetLabelText(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var days = CountWeekWorkDays(_item.Start, _item.End);

            return $"{days}d";
        }

        private int CountWeekWorkDays(DateTime start, DateTime end)
        {
            var days = (end - start).Days + 1;

            return WorkDaysInFullWeek(days) + WorkDaysInPartialWeek(start.DayOfWeek, days);
        }

        private int WorkDaysInFullWeek(int totalDays)
        {
            return totalDays / 7 * 5;
        }

        private int WorkDaysInPartialWeek(DayOfWeek firstDay, int totalDays)
        {
            var remainingDays = totalDays % 7;
            var daysToSaturday = (int)DayOfWeek.Saturday - (int)firstDay;

            if (remainingDays <= daysToSaturday)
                return remainingDays;

            if (remainingDays <= daysToSaturday + 2)
                return daysToSaturday;

            return remainingDays - 2;
        }
    }
}