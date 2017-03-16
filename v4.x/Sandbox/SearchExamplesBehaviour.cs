using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using SciChart.Wpf.UI.Reactive;
using SciChart.Wpf.UI.Reactive.Async;
using SciChart.Wpf.UI.Reactive.Extensions;
using SciChart.Wpf.UI.Reactive.Observability;
using SciChart.Wpf.UI.Reactive.Traits;

using ActionCommand = SciChart.Charting.Common.Helpers.ActionCommand;

namespace SciChart.Sandbox
{
    public class SearchExamplesBehaviour : ViewModelTrait<MainViewModel>
    {
        private List<ExampleCache> _exampleTypes;

        private class ExampleCache
        {
            public Type Type { get; set; }
            public TestCaseAttribute Attribute { get; set; }
        }

        public SearchExamplesBehaviour(MainViewModel target, ISchedulerContext schedulerContext) : base(target)
        {
            var assembly = typeof(MainWindow).Assembly;
            _exampleTypes = assembly.GetTypes()
                .Select(type => new ExampleCache { Type = type, Attribute = (TestCaseAttribute)type.GetCustomAttributes(typeof(TestCaseAttribute), false).FirstOrDefault() })
                .Where(tuple => tuple.Attribute != null)
                .ToList();

            Target.RunExampleCommand = new ActionCommand(OnRunExample, () => Target.SelectedExample != null);

            Target.WhenPropertyChanged(x => x.FilterText)
                .Throttle(TimeSpan.FromMilliseconds(250), schedulerContext.Default)
                .Select(DiscoverExamples)
                .ObserveOn(schedulerContext.Dispatcher)
                .Subscribe(UpdateExamples)
                .DisposeWith(this);
        }

        private void UpdateExamples(List<ExampleDataObject> examples)
        {
            Target.SelectedExample = null;
            Target.AllExamples = examples;
            Target.RunExampleCommand.RaiseCanExecuteChanged();
        }

        private List<ExampleDataObject> DiscoverExamples(string filterString)
        {
            var filteredExamples = _exampleTypes
                .Where(tuple => tuple.Attribute != null && (string.IsNullOrEmpty(filterString) || tuple.Attribute.TestCaseName.ToUpper().Like("*" + filterString.ToUpper() + "*")))
                .Select(tuple => new ExampleDataObject(tuple.Type, tuple.Attribute.TestCaseName))
                .OrderBy(example => example.Title)
                .ToList();

            return filteredExamples;
        }

        private void OnRunExample()
        {
            var obj = Target.SelectedExample;
            if (obj == null)
                return;
            var example = Activator.CreateInstance(obj.Type);

            Window w = example as Window;
            if (w == null)
            {
                w = new Window();
                w.Content = example;
            }

            w.Title = obj.Title;
            w.Show();
        }
    }
}