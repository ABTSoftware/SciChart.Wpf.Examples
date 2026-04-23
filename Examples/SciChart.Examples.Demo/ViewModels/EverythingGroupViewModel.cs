// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// EverythingGroupViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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