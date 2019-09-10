using System.Windows.Input;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Demo.ViewModels
{
    public class BreadcrumbItemViewModel : BaseViewModel
    {
        private string _content;
        public ICommand Command { get; private set; }

        public BreadcrumbItemViewModel(string content, ICommand command)
        {
            Content = content;
            Command = command;
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
    }
}