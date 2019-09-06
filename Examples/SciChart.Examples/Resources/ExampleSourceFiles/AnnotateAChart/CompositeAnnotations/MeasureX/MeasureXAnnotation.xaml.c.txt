// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MeasureXAnnotation.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using SciChart.Charting.Numerics.CoordinateCalculators;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.MeasureX
{
    public partial class MeasureXAnnotation : CompositeAnnotation
    {
        public MeasureXAnnotation()
        {
            InitializeComponent();
        }

        public override void Update(ICoordinateCalculator<double> xCoordCalc, ICoordinateCalculator<double> yCoordCalc)
        {
            base.Update(xCoordCalc, yCoordCalc);

            if (Y1.CompareTo(Y2) > 0)
            {
                MeasureText.VerticalAnchorPoint = VerticalAnchorPoint.Top;
                MeasureText.Margin = new Thickness(0, 5, 0, 0);
            }
            else
            {
                MeasureText.VerticalAnchorPoint = VerticalAnchorPoint.Bottom;
                MeasureText.Margin = new Thickness(0, -5, 0, 0);
            }

            var range = X1.CompareTo(X2) > 0
                ? RangeFactory.NewRange(X2, X1)
                : RangeFactory.NewRange(X1, X2);

            if (xCoordCalc is ICategoryCoordinateCalculator<DateTime>)
            {
                var categoryCalc = (ICategoryCoordinateCalculator<DateTime>) xCoordCalc;
                var indexRange = (IntegerRange)range;
                var difference = categoryCalc.TransformIndexToData(indexRange.Max) - categoryCalc.TransformIndexToData(indexRange.Min);
                MeasureText.Text = string.Format("{0:dd} days", difference);
            }
            else
            {
                MeasureText.Text = range.Diff.ToString();
            }
        }
    }
}