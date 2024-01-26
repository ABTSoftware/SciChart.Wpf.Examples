// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomAxisInfoTemplateSelector.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;

using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Oscilloscope
{
    public class CustomAxisInfoTemplateSelector : AxisInfoTemplateSelector
    {
        public static readonly DependencyProperty EmptyDataTemplateProperty = DependencyProperty.Register("EmptyDataTemplate", typeof(DataTemplate), typeof(CustomAxisInfoTemplateSelector), new PropertyMetadata(OnDefautlTemplateDependencyPropertyChanged));

        public DataTemplate EmptyDataTemplate
        {
            get { return (DataTemplate) GetValue(EmptyDataTemplateProperty); }
            set { SetValue(EmptyDataTemplateProperty, value); }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var axisInfo = item as AxisInfo;

            var dataTemplate = base.SelectTemplate(item, container);

            if (axisInfo != null)
            {
                if (axisInfo.AxisId == "DefaultAxisId")
                {
                    dataTemplate = axisInfo.IsXAxis ? XAxisDataTemplate : YAxisDataTemplate;
                }
                else
                {
                    dataTemplate = EmptyDataTemplate;
                }
            }

            return dataTemplate;
        }
    }
}