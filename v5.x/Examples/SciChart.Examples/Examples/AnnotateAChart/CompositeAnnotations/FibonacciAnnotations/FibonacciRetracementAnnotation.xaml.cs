// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FibonacciRetracementAnnotation.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Media;
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.FibonacciAnnotations
{
    public partial class FibonacciRetracementAnnotation : CompositeAnnotation
    {
        private readonly RatioModel[] _ratios =
        {
            new RatioModel(0d, new SolidColorBrush(Color.FromArgb(0xFF, 0x87, 0x77, 0x77))),
            new RatioModel(0.236, new SolidColorBrush(Color.FromArgb(0xFF, 0xC7, 0x38, 0x28))),
            new RatioModel(0.382, new SolidColorBrush(Color.FromArgb(0xFF, 0x8A, 0xCC, 0x28))),
            new RatioModel(0.5, new SolidColorBrush(Color.FromArgb(0xFF, 0x28, 0xCC, 0x33))),
            new RatioModel(0.618, new SolidColorBrush(Color.FromArgb(0xFF, 0x28, 0xC7, 0x9A))),
            new RatioModel(0.764, new SolidColorBrush(Color.FromArgb(0xFF, 0x31, 0x93, 0xC5))),
            new RatioModel(1d, new SolidColorBrush(Color.FromArgb(0xFF, 0x77, 0x77, 0x87))),
        };

        public FibonacciRetracementAnnotation()
        {
            InitializeComponent();
            DataContext = new FibonacciRetracementViewModel(_ratios);
        }
    }
}