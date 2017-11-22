using System.Collections.Generic;
using SciChart.Charting.Common.Helpers;
using SciChart.Wpf.UI.Reactive.Observability;

namespace SciChart.Sandbox
{
    public class MainViewModel : ViewModelWithTraitsBase
    {
        public MainViewModel()
        {
            this.WithTrait<SearchExamplesBehaviour>();
        }

        public ActionCommand RunExampleCommand
        {
            get { return GetDynamicValue<ActionCommand>("RunExampleCommand"); }
            set { SetDynamicValue("RunExampleCommand", value); }
        }

        public string FilterText
        {
            get { return GetDynamicValue<string>("FilterText"); }
            set { SetDynamicValue("FilterText", value); }
        }

        public IEnumerable<ExampleDataObject> AllExamples
        {
            get { return GetDynamicValue<IEnumerable<ExampleDataObject>>("AllExamples"); }
            set { SetDynamicValue("AllExamples", value); }
        }

        public ExampleDataObject SelectedExample
        {
            get { return GetDynamicValue<ExampleDataObject>("SelectedExample"); }
            set { SetDynamicValue("SelectedExample", value); }
        }        
    }
}
