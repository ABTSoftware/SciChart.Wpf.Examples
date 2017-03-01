using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Helpers.Grouping
{
    public class GroupingByFeature: IGrouping
    {
        public GroupingMode GroupingMode { get; set; }

        public GroupingByFeature()
        {
            GroupingMode = GroupingMode.Feature;
        }

        public ObservableCollection<TileViewModel> GroupingPredicate(IDictionary<Guid, Example> examples)
        {
            var temp = examples.SelectMany(pair =>
            {
                var l = new List<Tuple<Features, Example>>();
                pair.Value.Features.ForEach(feature => l.Add(new Tuple<Features, Example>(feature, pair.Value)));
                return l;
            });
            
            var groups = temp.OrderBy(pair => pair.Item1.ToString()).GroupBy(pair => pair.Item1);

            var result = new ObservableCollection<TileViewModel>();
            foreach (IGrouping<Features, Tuple<Features, Example>> pairs in groups)
            {
                result.Add(new TileViewModel
                {
                    TileDataContext = new EverythingGroupViewModel {GroupingName = pairs.Key.ToString()}
                });
                foreach (var example in pairs.Select(x => x.Item2))
                {
                    result.Add(new TileViewModel { TileDataContext = example });
                }
            }

            return result;
        }
    }
}