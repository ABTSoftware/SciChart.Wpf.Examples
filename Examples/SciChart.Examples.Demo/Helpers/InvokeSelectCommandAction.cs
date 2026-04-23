// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// InvokeSelectCommandAction.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
using SciChart.Examples.ExternalDependencies.Controls.CoverFlow;

namespace SciChart.Examples.Demo.Helpers
{
    /* NOTE:
     * We use Microsoft.Xaml.Behaviors.Behavior as a base class for this behaviour. We have embedded the source for
     * MS Behaviours in our SciChart.Examples.ExternalDependencies DLL for example purposes only and for compatibility with 
     * WPF and .NET Core
     *
     * What you should do is reference either System.Windows.Interactivity or Microsoft.Xaml.Behaviors.Wpf from NuGet
     * as it is not recommended to reference SciChart.Examples.ExternalDependencies in your applications 
     */

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
