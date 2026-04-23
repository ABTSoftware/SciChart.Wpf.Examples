// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ExampleGroupViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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