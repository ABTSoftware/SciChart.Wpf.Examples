// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ContentSizeLimiter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Demo.Controls
{
    public class ContentSizeLimiter : ContentControl
    {
        public static DependencyProperty ContentHeightLimitProperty = DependencyProperty.Register
            (nameof(ContentHeightLimit), typeof(double), typeof(ContentSizeLimiter), new PropertyMetadata(double.NaN));

        public static DependencyProperty ContentWidthLimitProperty = DependencyProperty.Register
            (nameof(ContentWidthLimit), typeof(double), typeof(ContentSizeLimiter), new PropertyMetadata(double.NaN));

        public static DependencyProperty AfterLimitVerticalContentAlignmentProperty = DependencyProperty.Register
            (nameof(AfterLimitVerticalContentAlignment), typeof(VerticalAlignment), typeof(ContentSizeLimiter),
            new PropertyMetadata(VerticalAlignment.Center));

        public static DependencyProperty AfterLimitHorizontalContentAlignmentProperty = DependencyProperty.Register
           (nameof(AfterLimitHorizontalContentAlignment), typeof(HorizontalAlignment), typeof(ContentSizeLimiter),
            new PropertyMetadata(HorizontalAlignment.Center));

        public double ContentHeightLimit
        {
            get => (double)GetValue(ContentHeightLimitProperty);
            set => SetValue(ContentHeightLimitProperty, value);
        }

        public double ContentWidthLimit
        {
            get => (double)GetValue(ContentWidthLimitProperty);
            set => SetValue(ContentWidthLimitProperty, value);
        }

        public VerticalAlignment AfterLimitVerticalContentAlignment
        {
            get => (VerticalAlignment)GetValue(AfterLimitVerticalContentAlignmentProperty);
            set => SetValue(AfterLimitVerticalContentAlignmentProperty, value);
        }

        public HorizontalAlignment AfterLimitHorizontalContentAlignment
        {
            get => (HorizontalAlignment)GetValue(AfterLimitHorizontalContentAlignmentProperty);
            set => SetValue(AfterLimitHorizontalContentAlignmentProperty, value);
        }

        public ContentSizeLimiter()
        {
            SizeChanged += OnContentSizeLimiterSizeChanged;
        }

        private void OnContentSizeLimiterSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Content is FrameworkElement fe)
            {
                if (e.WidthChanged && !ContentWidthLimit.IsNaN())
                {
                    if (ActualWidth > ContentWidthLimit)
                    {
                        fe.Width = ContentWidthLimit;
                        fe.HorizontalAlignment = AfterLimitHorizontalContentAlignment;
                    }
                    else
                    {
                        fe.Width = double.NaN;
                        fe.HorizontalAlignment = HorizontalContentAlignment;
                    }
                }

                if (e.HeightChanged && !ContentHeightLimit.IsNaN())
                {
                    if (ActualHeight > ContentHeightLimit)
                    {
                        fe.Height = ContentHeightLimit;
                        fe.VerticalAlignment = AfterLimitVerticalContentAlignment;
                    }
                    else
                    {
                        fe.Height = double.NaN;
                        fe.VerticalAlignment = VerticalContentAlignment;
                    }
                }
            }
        }
    }
}