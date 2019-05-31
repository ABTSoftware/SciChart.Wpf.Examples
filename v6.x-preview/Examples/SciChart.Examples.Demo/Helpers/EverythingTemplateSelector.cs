using System.Windows;

using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Helpers
{
    public class EverythingTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _exampleDataTemplate;
        private DataTemplate _groupDataTemplate;

        public DataTemplate ExampleDataTemplate
        {
            get { return _exampleDataTemplate; }
            set
            {
                _exampleDataTemplate = value;
                UpdateControlTemplate();
            }
        }

        public DataTemplate GroupDataTemplate
        {
            get { return _groupDataTemplate; }
            set
            {
                _groupDataTemplate = value;
                UpdateControlTemplate();
            }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dataTemplate = base.SelectTemplate(item, container);

            var tile = item as TileViewModel;
            if (tile != null)
            {
                if (tile.TileDataContext is Example)
                {
                    dataTemplate = ExampleDataTemplate;
                }
                else if (tile.TileDataContext is EverythingGroupViewModel)
                {
                    dataTemplate = GroupDataTemplate;
                }
            }

            return dataTemplate;
        }
    }
}