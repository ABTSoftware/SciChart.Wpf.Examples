// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FibonacciRetracementViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.FibonacciAnnotations
{
    public class RatioModel
    {
        public RatioModel(double value, Brush brush)
        {
            Value = value;
            Brush = brush;
        }

        public double Value { get; set; }
        public Brush Brush { get; set; }
    }

    public class FibonacciRetracementViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;

        public FibonacciRetracementViewModel()
        {
            Annotations = new ObservableCollection<IAnnotation>();
        }

        public FibonacciRetracementViewModel(params RatioModel[] ratios) : this()
        {
            Annotations.Add(CreateFibonacciRatioLine(ratios[0].Value, ratios[0].Brush));

            for (int i = 1; i < ratios.Length; i++)
            {
                Annotations.Add(CreateFibonacciRatioBox(ratios[i - 1].Value, ratios[i].Value, ratios[i].Brush));
                Annotations.Add(CreateFibonacciRatioLine(ratios[i].Value, ratios[i].Brush));
            }

            Annotations.Add(CreateTrendLine(new DoubleCollection {2d, 4d}, 2d, new SolidColorBrush(Color.FromArgb(0xFF, 0xBA, 0xBA, 0xBA))));
        }

        /// <summary>
        /// Gets or sets the <see cref="ObservableCollection{T}"/> 
        /// </summary>
        public ObservableCollection<IAnnotation> Annotations { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private IAnnotation CreateFibonacciRatioLine(double y1, Brush stroke)
        {
            return new FibonacciRatioLine
            {
                DataContext = new FibonacciRatioViewModel(y1, stroke)
            };
        }

        private IAnnotation CreateFibonacciRatioBox(double y1, double y2, Brush background)
        {
            return new FibonacciRatioBox
            {
                DataContext = new FibonacciRatioBoxViewModel(y1, y2, background)
            };
        }

        private IAnnotation CreateTrendLine(DoubleCollection strokeDashArray, double strokeThickness, Brush stroke)
        {
            return new TrendLine
            {
                DataContext = new TrendLineViewModel(strokeDashArray, strokeThickness, stroke)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}