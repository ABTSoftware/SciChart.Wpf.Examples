// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// TradeAnnotations.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.Examples.AnnotateAChart.TradeAnnotations
{
    /// <summary>
    /// Interaction logic for TradeAnnotations.xaml
    /// </summary>
    public partial class TradeAnnotations : UserControl
    {
        public TradeAnnotations()
        {
            InitializeComponent();
            ManipulationMargins.AnnotationLineWidth = 20d;
        }

        // Code for dragging the thumb on the toolbar. Not related to functionality of SciChart 
        private void Thumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (e.Source is Thumb thumb && thumb.Parent is Canvas canvas)
            {
                var left = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var top = Canvas.GetTop(thumb) + e.VerticalChange;

                if (left <= 0d)
                {
                    Canvas.SetLeft(thumb, 0d);
                }
                else if (left + thumb.ActualWidth <= canvas.ActualWidth)
                {
                    Canvas.SetLeft(thumb, left);
                }

                if (top <= 0d)
                {
                    Canvas.SetTop(thumb, 0d);
                }
                else if (top + thumb.ActualHeight <= canvas.ActualHeight)
                {
                    Canvas.SetTop(thumb, top);
                }
            }
        }
    }
}