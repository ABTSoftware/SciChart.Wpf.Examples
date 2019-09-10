using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Helpers.Grouping
{
    public interface IGrouping
    {
        GroupingMode GroupingMode { get; set; }
        ObservableCollection<TileViewModel> GroupingPredicate(IDictionary<Guid, Example> examples);
    }
}