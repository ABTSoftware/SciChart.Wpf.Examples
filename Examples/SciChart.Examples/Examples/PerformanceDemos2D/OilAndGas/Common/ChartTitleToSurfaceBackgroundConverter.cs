// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ChartTitleToSurfaceBackgroundConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common
{
    public class ChartTitleToSurfaceBackgroundConverter : IValueConverter
    {
        public Brush ShaleBrush { get; set; }

        public Brush DensityBrush { get; set; }
        
        public Brush ResistivityBrush { get; set; }
        
        public Brush SpaceBrush { get; set; }
        
        public Brush SonicBrush { get; set; }
        
        public Brush TextureBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string title)
            {
                switch (title)
                {
                    case "Shale": return ShaleBrush;
                    case "Density": return DensityBrush;
                    case "Resistivity": return ResistivityBrush;
                    case "Pore Space": return SpaceBrush;
                    case "Sonic": return SonicBrush;
                    case "Texture": return TextureBrush;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}