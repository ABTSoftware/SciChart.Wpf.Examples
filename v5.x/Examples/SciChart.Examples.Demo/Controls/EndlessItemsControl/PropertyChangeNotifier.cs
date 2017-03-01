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