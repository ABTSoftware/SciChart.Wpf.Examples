using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Demo.Behaviors
{
    /* NOTE:
     * We use Microsoft.Xaml.Behaviors.Behavior as a base class for this behaviour. We have embedded the source for
     * MS Behaviours in our SciChart.Examples.ExternalDependencies DLL for example purposes only and for compatibility with 
     * WPF and .NET Core
     *
     * What you should do is reference either System.Windows.Interactivity or Microsoft.Xaml.Behaviors.Wpf from NuGet
     * as it is not recommended to reference SciChart.Examples.ExternalDependencies in your applications 
     */

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
