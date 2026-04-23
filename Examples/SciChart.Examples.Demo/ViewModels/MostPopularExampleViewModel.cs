// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// MostPopularExampleViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows.Input;
using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.Common;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.UI.Reactive.Services;

namespace SciChart.Examples.Demo.ViewModels
{
    public class MostPopularExampleViewModel : ISelectable
    {
        public MostPopularExampleViewModel()
        {
            SelectCommand = new ActionCommand(() =>
            {
                if (Navigator.Instance.CanNavigateTo(AppPage.ExamplesPageId))
                {
                    Navigator.Instance.Navigate(AppPage.ExamplesPageId);
                }
                Example.SelectCommand.Execute(Example);
            });
        }

        public MostPopularExampleViewModel(Example example)
            : this()
        {
            Example = example;
            Rating = new ExampleRating();
        }

        public MostPopularExampleViewModel(Example example, ExampleRating rating)
            : this()
        {
            Example = example;
            Rating = rating;
        }

        public ExampleRating Rating { get; set; }

        public Example Example { get; set; }

        public ICommand SelectCommand { get; set; }
    }
}