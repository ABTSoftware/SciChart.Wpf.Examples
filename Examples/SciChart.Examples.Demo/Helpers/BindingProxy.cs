// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BindingProxy.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;

namespace SciChart.Examples.Demo.Helpers
{
    public class BindingProxy : Freezable
    {
         public static readonly DependencyProperty SourceProperty = DependencyProperty.Register
            (nameof(Source), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        } 
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}