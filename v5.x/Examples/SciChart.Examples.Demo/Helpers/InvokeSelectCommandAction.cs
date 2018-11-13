using System.Windows.Controls;
using System.Windows.Interactivity;
using SciChart.Examples.ExternalDependencies.Controls.CoverFlow;

namespace SciChart.Examples.Demo.Helpers
{
    public class InvokeSelectCommandAction : TriggerAction<Control>
    {
        protected override void Invoke(object parameter)
        {
            if (parameter is SelectionChangedEventArgs)
            {
                var eventArgs = parameter as SelectionChangedEventArgs;

                var selectedExample = (eventArgs.AddedItems.Count > 0 ? eventArgs.AddedItems[0] : null) as ISelectable;

                if (selectedExample != null)
                {
                    selectedExample.SelectCommand.Execute(selectedExample);
                }
            }
            else if (parameter is CoverFlowEventArgs)
            {
                var eventArgs = parameter as CoverFlowEventArgs;

                if (eventArgs.Item != null)
                {
                    var categoryViewModel = (eventArgs.Item) as ISelectable;
                    if (categoryViewModel != null && categoryViewModel.SelectCommand != null)
                    {
                        categoryViewModel.SelectCommand.Execute(categoryViewModel);
                    }
                }
            }
        }
    }
}
