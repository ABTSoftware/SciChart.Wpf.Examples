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