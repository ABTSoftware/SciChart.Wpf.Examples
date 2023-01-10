// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MeasureYAnnotation.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using SciChart.Charting.Numerics.CoordinateCalculators;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.MeasureY
{
    public partial class MeasureYAnnotation : CompositeAnnotation
    {
        public MeasureYAnnotation()
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

            var range = Y1.CompareTo(Y2) > 0
                ? RangeFactory.NewRange(Y2, Y1)
                : RangeFactory.NewRange(Y1, Y2);

            MeasureText.Text = string.Format("{0:#.##}", range.Diff);
        }
    }
}