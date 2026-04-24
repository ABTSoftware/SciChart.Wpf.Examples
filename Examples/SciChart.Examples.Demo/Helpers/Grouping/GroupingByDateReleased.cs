// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// GroupingByDateReleased.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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