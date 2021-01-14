// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ContentPresenterHelper.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
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
