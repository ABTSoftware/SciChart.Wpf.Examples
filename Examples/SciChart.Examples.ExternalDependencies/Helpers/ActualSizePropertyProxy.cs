// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ActualSizePropertyProxy.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.ComponentModel;
using System.Windows;

namespace SciChart.Examples.ExternalDependencies.Helpers
{
    public class ActualSizePropertyProxy : FrameworkElement, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FrameworkElement Element
        {
            get { return (FrameworkElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        public double ActualHeightValue
        {
            get { return Element == null ? 0 : Element.ActualHeight; }
        }

        public double ActualWidthValue
        {
            get { return Element == null ? 0 : Element.ActualWidth; }
        }

        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(ActualSizePropertyProxy),
                                        new PropertyMetadata(null, OnElementPropertyChanged));

        private static void OnElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ActualSizePropertyProxy)d).OnElementChanged(e);
        }

        private void OnElementChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldElement = (FrameworkElement)e.OldValue;
            var newElement = (FrameworkElement)e.NewValue;

            newElement.SizeChanged += OnElementSizeChanged;
            if (oldElement != null)
            {
                oldElement.SizeChanged -= OnElementSizeChanged;
            }

            NotifyPropChange();
        }

        private void OnElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotifyPropChange();
        }

        private void NotifyPropChange()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ActualWidthValue"));
                PropertyChanged(this, new PropertyChangedEventArgs("ActualHeightValue"));
            }
        }
    }

}
