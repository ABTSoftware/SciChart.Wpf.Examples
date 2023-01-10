using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Views
{
    public class ComboBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SelectedItemTemplate { get; set; }
        public DataTemplate DropDownItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement fe && fe.TemplatedParent is ComboBox)
            {
                return SelectedItemTemplate;       
            }
            return DropDownItemTemplate;
        }
    }
}