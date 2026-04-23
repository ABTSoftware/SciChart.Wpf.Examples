// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ExampleTreeNodeViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Examples.Demo.Helpers;
using System.Collections.Generic;

namespace SciChart.Examples.Demo.ViewModels
{
    public class ExampleTreeNodeViewModel
    {
        public bool ShowExpander { get; set; }

        public bool IsTopLevel => string.IsNullOrEmpty(GroupName);

        public string Name { get; }

        public string GroupName { get; }

        public List<ExampleTreeNodeViewModel> Children { get; }

        public Example Example { get; }

        public bool IsSelectable => !IsTopLevel;

        public ExampleTreeNodeViewModel(string name, string groupName, Example example)
        {
            Name = name;
            GroupName = groupName;
            Children = new List<ExampleTreeNodeViewModel>();
            Example = example;
        }

        public ExampleTreeNodeViewModel(string name, string groupName) : this(name, groupName, null)
        {
        }
    }
}