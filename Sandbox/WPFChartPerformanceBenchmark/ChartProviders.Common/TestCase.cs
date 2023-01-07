using System;
using System.Windows;

namespace ChartProviders.Common
{
    public class TestCase : IDisposable
    {
        private readonly int _i;
        private readonly string _version;
        private readonly ISpeedTest _speedTest;
        private readonly TimeSpan _duration;
        private readonly TestParameters _args;
        private readonly string _testName;


        
        public TestCase(int i, string version, string testName, ISpeedTest speedTest, TimeSpan duration, TestParameters args)
        {
            _i = i;
            _version = version;
            _speedTest = speedTest;
            _duration = duration;
            _args = args;
            _testName = testName;
        }

        public void Run(Action<double> completed)
        {
            //Arction added memory garbage collecting 
            GC.Collect();
            GC.WaitForPendingFinalizers(); 
            
            _speedTest.Execute(_args, _duration, completed);
        }

        public string TestCaseName
        {
            get { return String.Format("{0} {1}", _testName, _args.ToString()); }
        }

        public FrameworkElement SpeedTestUi
        {
            get { return _speedTest.Element; }
        }

        public string Version
        {
            get { return _version; }
        }

        public int TestNumber
        {
            get { return _i; }
        }

        public void Dispose()
        {
            _speedTest.Dispose();
        }
    }
}