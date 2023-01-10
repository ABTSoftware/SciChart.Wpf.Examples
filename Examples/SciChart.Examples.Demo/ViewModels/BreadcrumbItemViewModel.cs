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