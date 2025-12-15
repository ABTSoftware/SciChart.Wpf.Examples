using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Helpers.Grouping
{
    public class GroupingByDateReleased : IGrouping
    {
        public GroupingMode GroupingMode { get; set; }

        public GroupingByDateReleased()
        {
            GroupingMode = GroupingMode.DateReleased;
        }

        public ObservableCollection<TileViewModel> GroupingPredicate(IDictionary<Guid, Example> examples)
        {
            var groupExamples = new ObservableCollection<TileViewModel>
            {
                new TileViewModel
                {
                    TileDataContext = new EverythingGroupViewModel
                    {
                        GroupingIndex = 0,
                        SubcategoryName = "Release Date"
                    }
                }
            };

            foreach (var example in examples.Select(x => x.Value))
            {
                groupExamples.Add(new TileViewModel { TileDataContext = example });
            }

            return groupExamples;
        }
    }
}