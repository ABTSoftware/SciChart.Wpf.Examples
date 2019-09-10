using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SciChart.Examples.Demo.Common.Converters;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Helpers.Grouping
{
    public class GroupingByCategory : IGrouping
    {
        public GroupingMode GroupingMode { get; set; }

        public GroupingByCategory()
        {
            GroupingMode = GroupingMode.Category;
        }

        public ObservableCollection<TileViewModel> GroupingPredicate(IDictionary<Guid, Example> examples)
        {
            var groups = examples
                .OrderBy(example => example.Value.Title)
                .GroupBy(example => example.Value.Group)
                .OrderBy(group => group.Key);

            var result = new ObservableCollection<TileViewModel>();
            foreach (IGrouping<string, KeyValuePair<Guid, Example>> pairs in groups)
            {
                result.Add(new TileViewModel
                {
                    TileDataContext = new EverythingGroupViewModel { GroupingName = pairs.Key }
                });

                foreach (var example in pairs.Select(x => x.Value))
                {
                    result.Add(new TileViewModel{TileDataContext = example});
                }
            }

            return result;
        }
    }
}