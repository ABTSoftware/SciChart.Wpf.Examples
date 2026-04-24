// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PropertyChangeNotifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Controls.EndlessItemsControl
{
    public class PropertyChangeNotifier
    {
        static int _index;
        readonly DependencyProperty _propertySource;
        FrameworkElement _propertyTarget;

        public PropertyChangeNotifier()
        {
            _propertySource = DependencyProperty.RegisterAttached("DependencyPropertyListener" + _index++, typeof (object),
                typeof(PropertyChangeNotifier), new PropertyMetadata(null, HandleValueChanged));
        }

        public event EventHandler PropertyChanged;

        void HandleValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            OnPropertyChanged();
        }

        public void Attach(FrameworkElement element, Binding binding)
        {
            if (_propertyTarget != null)
            {
                throw new Exception("Cannot attach an already attached listener");
            }

            _propertyTarget = element;
            _propertyTarget.SetBinding(_propertySource, binding);
        }

        public void Detach()
        {
            _propertyTarget.ClearValue(_propertySource);
            _propertyTarget = null;
        }

        protected void OnPropertyChanged()
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(_propertyTarget, EventArgs.Empty);
            }
        }
    }
}