using System;
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Charting3D.Axis;
using SciChart.Core.Extensions;

namespace DoubleAxisAsDateTimeAxisExample
{
    public class OADateLabelProvider : LabelProviderBase
    {
        public override string FormatCursorLabel(IComparable dataValue)
        {
            var oaDateTime = dataValue.ToDouble();
            var dateTime = DateTime.FromOADate(oaDateTime);
            var formattedText = ParentAxis.CursorTextFormatting.IsNullOrEmpty()
                ? FormatLabel(dataValue)
                : dateTime.ToString(ParentAxis.CursorTextFormatting);

            return formattedText;
        }

        public override string FormatLabel(IComparable dataValue)
        {
            var nAxis = ParentAxis as NumericAxis3D;
            if (nAxis == null)
            {
                throw new InvalidOperationException("The DateTimeLabelFormatter is only valid on instances of DateTimeAxis");
            }

            var oadt = dataValue.ToDouble();
            var dt = DateTime.FromOADate(oadt);

            return dt.ToString("hh:mm:ss");
        }
    }
}
