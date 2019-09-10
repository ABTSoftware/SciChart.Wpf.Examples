using System.Windows.Input;

namespace SciChart.Examples.Demo.Helpers
{
    public interface ISelectable
    {
        ICommand SelectCommand { get; set; }
    }
}
