using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Controls
{
    public class CustomListBox : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomListBoxItem();
        }
    }
}