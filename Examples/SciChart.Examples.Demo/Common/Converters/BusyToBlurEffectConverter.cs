// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BusyToBlurEffectConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class BusyToBlurEffectConverter : IValueConverter
    {
        private readonly BlurEffect _blurEffect = new BlurEffect();

        public double BlurRadius { get; set; } = 5;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBusy)
            {
                if (_blurEffect.Radius != BlurRadius && BlurRadius >= 0)
                {
                    _blurEffect.Radius = BlurRadius;
                }
                return isBusy ? _blurEffect : null;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}