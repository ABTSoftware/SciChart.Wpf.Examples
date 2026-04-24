// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BreadcrumbItemViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows.Input;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Demo.ViewModels
{
    public class BreadcrumbItemViewModel : BaseViewModel
    {
        private string _content;

        private bool _isSelected;

        public ICommand Command { get; private set; }

        public BreadcrumbItemViewModel(string content, ICommand command)
        {
            Content = content;
            Command = command;
        }

        public BreadcrumbItemViewModel(string content, ICommand command, bool isSelected) : this(content, command)
        {
            IsSelected = isSelected;
        }

        public string Content 
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        public bool IsSelected 
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
}