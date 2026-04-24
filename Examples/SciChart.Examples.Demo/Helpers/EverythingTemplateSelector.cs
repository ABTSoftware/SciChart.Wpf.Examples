// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// EverythingTemplateSelector.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;

using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.ViewModels;

namespace SciChart.Examples.Demo.Helpers
{
    public class EverythingTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _exampleDataTemplate;
        private DataTemplate _groupDataTemplate;

        public DataTemplate ExampleDataTemplate
        {
            get { return _exampleDataTemplate; }
            set
            {
                _exampleDataTemplate = value;
                UpdateControlTemplate();
            }
        }

        public DataTemplate GroupDataTemplate
        {
            get { return _groupDataTemplate; }
            set
            {
                _groupDataTemplate = value;
                UpdateControlTemplate();
            }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dataTemplate = base.SelectTemplate(item, container);

            var tile = item as TileViewModel;
            if (tile != null)
            {
                if (tile.TileDataContext is Example)
                {
                    dataTemplate = ExampleDataTemplate;
                }
                else if (tile.TileDataContext is EverythingGroupViewModel)
                {
                    dataTemplate = GroupDataTemplate;
                }
            }

            return dataTemplate;
        }
    }
}