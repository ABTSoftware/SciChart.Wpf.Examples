using System.Windows.Input;
using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.Helpers;

namespace SciChart.Examples.Demo.ViewModels
{
    public class EverythingGroupViewModel : ISelectable
    {
        public EverythingGroupViewModel()
        {
            SelectCommand = new ActionCommand(() => {});
        }

        public int GroupingIndex { get; set; }

        public string SubcategoryName { get; set; }

        public ICommand SelectCommand { get; set; }

        public string CategoryName { get; set; }

        public int ExamplesCount { get; set; }
    }
}