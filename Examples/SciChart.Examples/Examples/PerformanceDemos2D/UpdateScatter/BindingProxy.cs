using System.Windows;

namespace SciChart.Examples.Examples.PerformanceDemos2D.UpdateScatter
{
    internal class BindingProxy : FrameworkElement
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingSourceProperty =
            DependencyProperty.Register(nameof(BindingSource), typeof(object), typeof(BindingProxy), new PropertyMetadata(null, OnBindingSourceChanged));

        // Using a DependencyProperty as the backing store for RenderSurfaceTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingTargetProperty =
            DependencyProperty.Register(nameof(BindingTarget), typeof(object), typeof(BindingProxy), new PropertyMetadata(null));

        public object BindingTarget
        {
            get { return (object)GetValue(BindingTargetProperty); }
            set { SetValue(BindingTargetProperty, value); }
        }

        public object BindingSource
        {
            get { return (object)GetValue(BindingSourceProperty); }
            set { SetValue(BindingSourceProperty, value); }
        }

        private static void OnBindingSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindingProxy proxy)
            {
                proxy.BindingTarget = e.NewValue;
            }
        }
    }
}
