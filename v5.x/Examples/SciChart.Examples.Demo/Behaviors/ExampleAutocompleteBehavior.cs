using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SciChart.Examples.Demo.Behaviors
{
    public class ExampleAutocompleteBehavior: Behavior<AutoCompleteBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.FilterMode = AutoCompleteFilterMode.StartsWith;
            //AssociatedObject.ItemFilter = ExampleSearchViewModel.IsAutocompleteSuggestion;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.FilterMode = AutoCompleteFilterMode.StartsWith;
            AssociatedObject.ItemFilter = null;
        }
    }
}
