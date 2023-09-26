using System.Collections.Generic;
using System.Windows.Input;
using SciChart.Examples.Demo.Helpers;

namespace SciChart.Examples.Demo.ViewModels
{
    public class ExampleCategoryViewModel : ISelectable
    {
        public string Category { get; set; }

        public bool IsHomeCategory { get; set; }

        public IEnumerable<string> Groups { get; set; }

        public ICommand SelectCommand { get; set; }
    }

    public class ExampleGroupViewModel : ISelectable
    {
        public string Group { get; set; }

        public IEnumerable<TileViewModel> Examples { get; set; }

        public ICommand SelectCommand { get; set; }
    }
}