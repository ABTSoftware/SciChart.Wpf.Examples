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