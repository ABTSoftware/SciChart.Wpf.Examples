using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Helpers.Grouping
{
    public class GroupingByMostUsed : IGrouping
    {
        public GroupingMode GroupingMode { get; set; }

        public GroupingByMostUsed()
        {
            GroupingMode = GroupingMode.MostUsed;
        }

        public ObservableCollection<TileViewModel> GroupingPredicate(IDictionary<Guid, Example> examples)
        {
            var result = new ObservableCollection<TileViewModel>
            {
                new TileViewModel {TileDataContext = new EverythingGroupViewModel {GroupingName = "MostUsedGroup"}}
            };

            foreach (var example in examples.Select(x => x.Value))
            {
                result.Add(new TileViewModel { TileDataContext = example });
            }

            return result;
        }
    }
}