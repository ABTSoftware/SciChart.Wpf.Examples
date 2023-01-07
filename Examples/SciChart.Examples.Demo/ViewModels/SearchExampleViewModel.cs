﻿using System.Windows.Input;

using Unity;
using SciChart.Charting.Common.Helpers;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.UI.Bootstrap;

namespace SciChart.Examples.Demo.ViewModels
{
    public class SearchExampleViewModel : ISelectable
    {
        public SearchExampleViewModel()
        {
            SelectCommand = new ActionCommand(() =>
            {
                if (Navigator.Instance.CanNavigateTo(AppPage.ExamplesPageId))
                {
                    Navigator.Instance.Navigate(AppPage.ExamplesPageId);
                }
                Example.SelectCommand.Execute(Example);
                ServiceLocator.Container.Resolve<IMainWindowViewModel>().SearchText = "";
            });
        }

        public Example Example { get; set; }

        public ICommand SelectCommand { get; set; }
    }
}