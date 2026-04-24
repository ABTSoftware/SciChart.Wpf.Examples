// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ContentPresenterHelper.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.ExternalDependencies.Behaviors
{
    public class ContentPresenterHelper
    {
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached(
            "ContentTemplate", typeof(string), typeof(ContentPresenterHelper), new PropertyMetadata(default(string), OnContentTemplateChanged));        

        public static void SetContentTemplate(DependencyObject element, string value)
        {
            element.SetValue(ContentTemplateProperty, value);
        }

        public static string GetContentTemplate(DependencyObject element)
        {
            return (string) element.GetValue(ContentTemplateProperty);
        }

        private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentPresenter p = d as ContentPresenter;
            if (p != null)
            {
                string resource = e.NewValue as string;
                if (resource == null)
                {
                    p.ContentTemplate = null;
                }
                else
                {
                    p.ContentTemplate = p.TryFindResource(resource) as DataTemplate;            
                }
            }
        }
    }
}
