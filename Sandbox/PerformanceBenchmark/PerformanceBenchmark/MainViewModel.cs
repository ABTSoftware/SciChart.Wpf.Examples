using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using ChartProviders.Common.Interfaces;
using ChartProviders.Common.Models;

namespace PerformanceBenchmark
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isSessionRunning;
        private readonly Queue<TestCase> _testQueue = new();
        private readonly IChartProvider[] _chartProviders;
        private readonly HeatProvider _heatProvider;

        public ObservableCollection<DataGridColumn> Columns { get; } = new();

        public ObservableCollection<string> TestRuns { get; } = new();

        public ObservableCollection<TestResult> TestResults { get; } = new();

        public bool HasTestRuns => TestRuns.Any();

        public bool IsSessionRunning
        {
            get => _isSessionRunning;
            private set
            {
                if (_isSessionRunning != value)
                {
                    _isSessionRunning = value;
                    OnPropertyChanged(nameof(IsSessionRunning));
                    OnPropertyChanged(nameof(IsSessionCompleted));
                }
            }
        }

        public bool IsSessionCompleted => !IsSessionRunning;

        public IAddChild LayoutRoot { get; set; }

        public MainViewModel()
        {
            // Maps FPS to brushes for grid
            _heatProvider = new HeatProvider(0, 60);

            TestRuns.CollectionChanged += (s, e) => OnPropertyChanged(nameof(HasTestRuns));

            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            var textBinding = new Binding("TestName")
            {
                Mode = BindingMode.OneWay,
                StringFormat = "N2"
            };

            textBlock.SetValue(TextBlock.PaddingProperty, new Thickness(3));
            textBlock.SetBinding(TextBlock.TextProperty, textBinding);

            var testNameColumn = new DataGridTemplateColumn
            {
                CellTemplate = new DataTemplate { VisualTree = textBlock },
                ClipboardContentBinding = textBinding,
                Header = "Test Name"
            };

            Columns.Add(testNameColumn);

            // Gets the Charting Providers (which test groups to run)
            _chartProviders = ChartProviderFactory.GetChartProviders();

            // Create columns
            foreach (var chartProvider in _chartProviders)
            {
                // On result, bind the cell background to output of _heatProvider;
                var resultBinding = new Binding($"[{chartProvider.Name}]")
                {
                    Mode = BindingMode.OneWay,
                    StringFormat = "N2"
                };

                var backgroundBinding = new Binding($"[{chartProvider.Name}]")
                {
                    Mode = BindingMode.OneWay,
                    Converter = new HeatConverter(_heatProvider)
                };

                textBlock = new FrameworkElementFactory(typeof(TextBlock));

                textBlock.SetValue(TextBlock.PaddingProperty, new Thickness(3));
                textBlock.SetBinding(TextBlock.BackgroundProperty, backgroundBinding);
                textBlock.SetBinding(TextBlock.TextProperty, resultBinding);

                Columns.Add(new DataGridTemplateColumn
                {
                    CellTemplate = new DataTemplate { VisualTree = textBlock },
                    ClipboardContentBinding = resultBinding,
                    Header = chartProvider.Name
                });
            }

            // Create test cases. We transpose the matrix of cases so they are performed in the order
            //
            // Test1   (1)Version1  (2)Version2  (3)Version3
            // Test2   (4)Version1  (5)Version2  (6)Version3
            //
            // to avoid problem of CPU heating causing throttling and later versions looking slower

            var allSpeedTests = _chartProviders.SelectMany(CreateTestCases).OrderBy(tc => tc.TestNumber);

            foreach (var speedTest in allSpeedTests)
            {
                _testQueue.Enqueue(speedTest);
            }
        }

        private IEnumerable<TestCase> CreateTestCases(IChartProvider chartProvider)
        {
            var index = 1;
            var duration = TimeSpan.FromSeconds(10);

            var listTestCases = new List<TestCase>();
            var resamplerMode = Resampling.Auto;
            var runnerType = TestRunnerType.Composition;

            // Testing the 500x500 series case (many series of few points) 
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "100x100, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 100, resamplerMode, true) { StrokeThickness = 1 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "500x500, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 500, resamplerMode, true) { StrokeThickness = 1 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "1000x1000, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 1000, resamplerMode, true) { StrokeThickness = 1 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "100x100, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 100, resamplerMode, true) { StrokeThickness = 2 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "500x500, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 500, resamplerMode, true) { StrokeThickness = 2 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "1000x1000, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 1000, resamplerMode, true) { StrokeThickness = 2 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "100x100, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 100, resamplerMode, true) { StrokeThickness = 5 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "500x500, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 500, resamplerMode, true) { StrokeThickness = 5 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "1000x1000, AA On", chartProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 1000, resamplerMode, true) { StrokeThickness = 5 }));

            // Scatter Series updating. Each test appends N points to a scatter series and a new N points each render. Tests raw fill-rate of the renderer
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Scatter", chartProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 1000) { DataDistribution = DataDistribution.Uniform }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Scatter", chartProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 5000) { DataDistribution = DataDistribution.Uniform }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Scatter", chartProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 10000) { DataDistribution = DataDistribution.Uniform }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Scatter", chartProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 25000) { DataDistribution = DataDistribution.Uniform }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Scatter", chartProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 100000) { DataDistribution = DataDistribution.Uniform }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Scatter", chartProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 250000) { DataDistribution = DataDistribution.Uniform }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Scatter", chartProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 500000) { DataDistribution = DataDistribution.Uniform }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Scatter", chartProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 1000000) { DataDistribution = DataDistribution.Uniform }));

            // FIFO Series updating. Tests speed of renderer, resampling and dataseries
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000, resamplerMode, true) { StrokeThickness = 1 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 100000, resamplerMode, true) { StrokeThickness = 1 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 1000000, resamplerMode, true) { StrokeThickness = 1 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000000, resamplerMode, true) { StrokeThickness = 1 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000, resamplerMode, true) { StrokeThickness = 2 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 100000, resamplerMode, true) { StrokeThickness = 2 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 1000000, resamplerMode, true) { StrokeThickness = 2 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000000, resamplerMode, true) { StrokeThickness = 2 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000, resamplerMode, true) { StrokeThickness = 3 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 100000, resamplerMode, true) { StrokeThickness = 3 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 1000000, resamplerMode, true) { StrokeThickness = 3 }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "FIFO", chartProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000000, resamplerMode, true) { StrokeThickness = 3 }));

            // Line Series Appending. Each test appends N lines to an existing series. Does not scroll
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 10000, 1000, 0, true, resamplerMode) { StrokeThickness = 1.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 1000, 1, true, resamplerMode) { StrokeThickness = 1.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 10000, 10, true, resamplerMode) { StrokeThickness = 1.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 1000000, 10000, 100, true, resamplerMode) { StrokeThickness = 1.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 10000, 1000, 0, true, resamplerMode) { StrokeThickness = 2.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 1000, 1, true, resamplerMode) { StrokeThickness = 2.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 10000, 10, true, resamplerMode) { StrokeThickness = 2.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 1000000, 10000, 100, true, resamplerMode) { StrokeThickness = 2.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 10000, 1000, 0, true, resamplerMode) { StrokeThickness = 5.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 1000, 1, true, resamplerMode) { StrokeThickness = 5.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 10000, 10, true, resamplerMode) { StrokeThickness = 5.0f }));
            listTestCases.Add(new TestCase(index++, chartProvider.Name, "Append", chartProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 1000000, 10000, 100, true, resamplerMode) { StrokeThickness = 5.0f }));

            return listTestCases;
        }

        public void RunNextTest()
        {
            if (_testQueue.Count == 0)
            {
                IsSessionRunning = false;
                return;
            }

            IsSessionRunning = true;

            var testCase = _testQueue.Dequeue();

            LayoutRoot.AddChild(testCase.TestContent);

            testCase.Run(result =>
            {
                // clear the test layout
                LayoutRoot.AddChild(null);

                // log the test run
                TestRuns.Add($"({result:N2}) - {testCase.TestName} - {testCase.Version}");

                // add results to the table
                var testResult = TestResults.SingleOrDefault(i => i.TestName == testCase.TestName);

                if (testResult == null)
                {
                    testResult = new TestResult
                    {
                        TestName = testCase.TestName
                    };

                    TestResults.Add(testResult);
                }

                testResult[testCase.Version] = result;

                WriteLog($"{testCase.TestName};{result:0.0};", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PerformanceDump.csv"));

                TestFinished(testCase);
            });
        }

        private void TestFinished(TestCase testCase)
        {
            testCase.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            RunNextTest();
        }

        public static void WriteLog(string entry, string fileName)
        {
            string strPath = fileName;

            StreamWriter sw = File.Exists(strPath)
                ? File.AppendText(strPath)
                : File.CreateText(strPath);

            using (sw)
            {
                sw.WriteLine($"{entry} [{DateTime.Now.ToLongTimeString()}]");
                sw.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
