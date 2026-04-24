// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomLabelProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows;
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.StyleAChart.UseLabelProvider
{
    public class CustomLabelProvider : LabelProviderBase
    {
        private readonly CustomLabelParams _labelParams;
        private readonly LabelColorGenerator _colorGenerator;

        public CustomLabelProvider(CustomLabelParams labelParams)
        {
            _labelParams = labelParams;
            _colorGenerator = new LabelColorGenerator(labelParams.ColorFrom, labelParams.ColorTo);
        }

        public bool IsValueInRange(double dataValue)
        {
            return dataValue >= _labelParams.ValueFrom && dataValue <= _labelParams.ValueTo;
        }

        public override ITickLabelViewModel CreateDataContext(IComparable dataValue)
        {
            return UpdateDataContext(new CustomLabelViewModel(), dataValue);
        }

        public override ITickLabelViewModel UpdateDataContext(ITickLabelViewModel labelDataContext, IComparable dataValue)
        {
            base.UpdateDataContext(labelDataContext, dataValue);

            var value = dataValue.ToDouble();
            var label = (CustomLabelViewModel)labelDataContext;

            label.Text = labelDataContext.Text;
            label.HasExponent = false;

            if (IsValueInRange(value))
            {
                label.FontSize = 18;
                label.FontWeight = FontWeights.Bold;
                label.Foreground = _colorGenerator.GetColor(value);
                label.Angle = _labelParams.LabelAngle;
            }
            else
            {
                label.FontSize = 15;
                label.FontWeight = FontWeights.Regular;
                label.Foreground = _colorGenerator.DefaultColor;
                label.Angle = 0;
            }
            
            return label;
        }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            return string.Format($"{{0:{ParentAxis.CursorTextFormatting}}}", dataValue);
        }

        public override string FormatLabel(IComparable dataValue)
        {
            return IsValueInRange(dataValue.ToDouble())
                ? string.Format(_labelParams.LabelFormat, dataValue)
                : string.Format($"{{0:{ParentAxis.TextFormatting}}}", dataValue);
        }
    }
}