using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Helpers.Grouping
{
    public class GroupingByFeature : IGrouping
    {
        public GroupingMode GroupingMode { get; set; }

        public GroupingByFeature()
        {
            GroupingMode = GroupingMode.Feature;
        }

        public ObservableCollection<TileViewModel> GroupingPredicate(IDictionary<Guid, Example> examples)
        {
            var groups = examples.SelectMany(pair =>
            {
                var list = new List<Tuple<Features, Example>>();
                pair.Value.Features.ForEach(feature => list.Add(new Tuple<Features, Example>(feature, pair.Value)));
                return list;
            })
            .OrderBy(pair => pair.Item1.ToString())
            .GroupBy(pair => pair.Item1);

            var groupIndex = -1;
            var groupExamples = new ObservableCollection<TileViewModel>();

            foreach (IGrouping<Features, Tuple<Features, Example>> pairs in groups)
            {
                groupIndex++;
                groupExamples.Add(new TileViewModel
                {
                    TileDataContext = new EverythingGroupViewModel
                    {
                        GroupingIndex = groupIndex,
                        GroupingName = pairs.Key.ToString()
                    }
                });

                foreach (var example in pairs.Select(x => x.Item2))
                {
                    groupExamples.Add(new TileViewModel { TileDataContext = example });
                }
            }

            return groupExamples;
        }
    }
}