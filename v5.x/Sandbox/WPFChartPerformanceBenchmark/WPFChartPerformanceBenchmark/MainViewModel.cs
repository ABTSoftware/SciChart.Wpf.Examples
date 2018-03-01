using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ChartProviders.Common;

namespace WPFChartPerformanceBenchmark
{
    public class MainViewModel : ViewModelBase
    {
        private Queue<TestCase> _testQueue = new Queue<TestCase>();
        private ObservableCollection<string> _testRuns = new ObservableCollection<string>();
        private ObservableCollection<TestResult> _testResults = new ObservableCollection<TestResult>();
        private ObservableCollection<DataGridColumn> _columns = new ObservableCollection<DataGridColumn>();
        private readonly IChartingProvider[] _chartProviders;
        private HeatProvider _heatProvider;

        public MainViewModel()
        {
            // Maps FPS to brushes for grid
            _heatProvider = new HeatProvider(0, 60);

            var testNameColumn = new DataGridTextColumn { Binding = new Binding("TestName"), Header = "Test Name" };
            _columns.Add(testNameColumn);

            // Gets the Charting Providers (which test groups to run)
            _chartProviders = TestSetup.GetChartProviders();

            // Create columns
            foreach (var chartingProvider in _chartProviders)
            {
                // On result, bind the cell background to output of _heatProvider;
                var resultBinding = new Binding("[" + chartingProvider.Name + "]")
                {
                    StringFormat = "N2"
                };

                var backgroundBinding = new Binding("[" + chartingProvider.Name + "]")
                {
                    Converter = new HeatConverter(_heatProvider)
                };

                var textBlock = new FrameworkElementFactory(typeof(TextBlock));
                textBlock.SetValue(TextBlock.MarginProperty, new Thickness(0));
                textBlock.SetValue(TextBlock.PaddingProperty, new Thickness(3));

                textBlock.SetBinding(TextBlock.BackgroundProperty, backgroundBinding);
                textBlock.SetBinding(TextBlock.TextProperty, resultBinding);
                var col = new DataGridTemplateColumn
                    {
                        CellTemplate = new DataTemplate() { VisualTree = textBlock },
                        Header = chartingProvider.Name
                    };
                _columns.Add(col);
            }

            // Create test cases. We transpose the matrix of cases so they are performed in the order
            //   Test1    (1)Version1    (2)Version2    (3)Version3
            //   Test3    (4)Version1    (5)Version2    (6)Version3
            // to avoid problem of CPU heating causing throttling and later versions looking slower

            var allSpeedTests = _chartProviders.SelectMany(this.CreateTestCases).OrderBy(tc => tc.TestNumber);
            foreach (var speedTest in allSpeedTests)
            {
                _testQueue.Enqueue(speedTest);
            }
        }

        public ObservableCollection<DataGridColumn> Columns
        {
            get { return _columns; }
            set
            {
                if (_columns == value) return;
                _columns = value;
                OnPropertyChanged("Columns");
            }
        }


        public ObservableCollection<string> TestRuns
        {
            get { return _testRuns; }
            set
            {
                if (_testRuns == value) return;
                _testRuns = value;
                OnPropertyChanged("TestRuns");
            }
        }

        public ObservableCollection<TestResult> Results
        {
            get { return _testResults; }
            set
            {
                if (_testResults == value) return;
                _testResults = value;
                OnPropertyChanged("Results");
            }
        }

        private IEnumerable<TestCase> CreateTestCases(IChartingProvider chartingProvider)
        {
            TimeSpan duration = TimeSpan.FromSeconds(10); // Per test
            int i = 1; // Test Number
            List<TestCase> listTestCases = new List<TestCase>();

            //ChartProviders.Common.Resampling sciChartResampling = Resampling.None; 
            ChartProviders.Common.Resampling sciChartResampling = Resampling.Auto; 
            
            TestRunnerType runnerType = TestRunnerType.Composition;
            // (TestRunnerType runnerType = TestRunnerType.DispatcherTimer; runnerType <= TestRunnerType.Composition; runnerType++)
            {
//                //Testing the 500x500 series case (many series of few points) 
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "100x100, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 100, sciChartResampling, true) { StrokeThickness = 1 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "500x500, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 500, sciChartResampling, true) { StrokeThickness = 1 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "1000x1000, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 1000, sciChartResampling, true) { StrokeThickness = 1 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "100x100, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 100, sciChartResampling, true) { StrokeThickness = 2 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "500x500, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 500, sciChartResampling, true) { StrokeThickness = 2 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "1000x1000, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 1000, sciChartResampling, true) { StrokeThickness = 2 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "100x100, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 100, sciChartResampling, true) { StrokeThickness = 5 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "500x500, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 500, sciChartResampling, true) { StrokeThickness = 5 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "1000x1000, AA On", chartingProvider.LoadNxNRefreshTest(), duration, new TestParameters(runnerType, 1000, sciChartResampling, true) { StrokeThickness = 5 }));
//                
                
                // Scatter Series updating. Each test appends N points to a scatter series and a new N points each render. Tests raw fill-rate of the renderer
                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "Scatter", chartingProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 1000) { DataDistribution = DataDistribution.Uniform }));
                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "Scatter", chartingProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 5000) { DataDistribution = DataDistribution.Uniform }));
                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "Scatter", chartingProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 10000) { DataDistribution = DataDistribution.Uniform }));
                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "Scatter", chartingProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 25000) { DataDistribution = DataDistribution.Uniform }));
                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "Scatter", chartingProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 100000) { DataDistribution = DataDistribution.Uniform }));
                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "Scatter", chartingProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 250000) { DataDistribution = DataDistribution.Uniform }));
                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "Scatter", chartingProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 500000) { DataDistribution = DataDistribution.Uniform }));
                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "Scatter", chartingProvider.ScatterPointsSpeedTest(), duration, new TestParameters(runnerType, 1000000) { DataDistribution = DataDistribution.Uniform })); 
//                
//                // FIFO Series updating. Tests speed of renderer, resampling and dataseries
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000, sciChartResampling, true) { StrokeThickness = 1 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 100000, sciChartResampling, true) { StrokeThickness = 1 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 1000000, sciChartResampling, true) { StrokeThickness = 1 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000000, sciChartResampling, true) { StrokeThickness = 1 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000, sciChartResampling, true) { StrokeThickness = 2 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 100000, sciChartResampling, true) { StrokeThickness = 2 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 1000000, sciChartResampling, true) { StrokeThickness = 2 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000000, sciChartResampling, true) { StrokeThickness = 2 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000, sciChartResampling, true) { StrokeThickness = 3 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 100000, sciChartResampling, true) { StrokeThickness = 3 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 1000000, sciChartResampling, true) { StrokeThickness = 3 }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "FIFO", chartingProvider.FifoLineSpeedTest(), duration, new TestParameters(runnerType, 10000000, sciChartResampling, true) { StrokeThickness = 3 })); 
//                                
//                //  Line Series Appending. Each test appends N lines to an existing series. Does not scroll
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 10000, 1000, 0, true, sciChartResampling) { StrokeThickness = 1.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 1000, 1, true, sciChartResampling) { StrokeThickness = 1.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 10000, 10, true, sciChartResampling) { StrokeThickness = 1.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 1000000, 10000, 100, true, sciChartResampling) { StrokeThickness = 1.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 10000, 1000, 0, true, sciChartResampling) { StrokeThickness = 2.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 1000, 1, true, sciChartResampling) { StrokeThickness = 2.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 10000, 10, true, sciChartResampling) { StrokeThickness = 2.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 1000000, 10000, 100, true, sciChartResampling) { StrokeThickness = 2.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 10000, 1000, 0, true, sciChartResampling) { StrokeThickness = 5.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 1000, 1, true, sciChartResampling) { StrokeThickness = 5.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 100000, 10000, 10, true, sciChartResampling) { StrokeThickness = 5.0f }));
//                listTestCases.Add(new TestCase(i++, chartingProvider.Name, "APPEND", chartingProvider.LineAppendSpeedTest(), duration, new LineAppendTestParameters(runnerType, 1000000, 10000, 100, true, sciChartResampling) { StrokeThickness = 5.0f }));            
            }

            
            return listTestCases.ToArray();

        }

        public Panel LayoutRoot { get; set; }
        
        int _iTestResult = 0; 

        public void RunNextTest()
        {
            if (_testQueue.Count == 0)
                return;

            var testCase = _testQueue.Dequeue();
            LayoutRoot.Children.Add(testCase.SpeedTestUi);

            testCase.Run((result) =>
                {
                    LayoutRoot.Children.Remove(testCase.SpeedTestUi);

                    // log the test run
                    _testRuns.Add(string.Format("({0:N2}) - {1} - {2}", result, testCase.TestCaseName, testCase.Version));

                    // add results to the table
                    var testResult = _testResults.SingleOrDefault(i => i.TestName == testCase.TestCaseName);
                    if (testResult == null)
                    {
                        testResult = new TestResult()
                        {
                            TestName = testCase.TestCaseName
                        };
                        _testResults.Add(testResult);
                    }
                    testResult[testCase.Version] = result;
                    string strPrefix = "LC";
                    if (_iTestResult % 2 == 0)
                    {
                        strPrefix = "SC";
                    }
                    else
                    {
                        strPrefix = "LC";
                    } 
                    //Write to log 
                    WriteLog(testCase.TestCaseName + ";" + result.ToString("0.0") + ";", System.AppDomain.CurrentDomain.BaseDirectory + "\\"+strPrefix+"_PerformanceComparisonDump.csv");
                    _iTestResult++; 
                    // start the next test
                    TestFinished(testCase, result);
                });
        }

        private void TestFinished(TestCase testCase, double result)
        {
            testCase.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            RunNextTest();
        }

        /// <summary>
        /// Write a string to log file. 
        /// </summary>
        /// <param name="entry">String to write</param>
        /// <param name="fileName">File name</param>
        public static void WriteLog(string entry, string fileName)
        {
            string strPath = fileName;
            System.IO.StreamWriter sw;
            if (System.IO.File.Exists(strPath))
                sw = System.IO.File.AppendText(strPath);
            else
                sw = System.IO.File.CreateText(strPath);
            if (sw != null)
            {
                sw.WriteLine(entry + "[" + DateTime.Now.ToLongTimeString() + "]");
                sw.Close();
            }

        }
    }
}
