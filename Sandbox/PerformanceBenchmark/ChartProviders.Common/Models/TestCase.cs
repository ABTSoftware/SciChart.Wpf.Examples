using System;
using System.Windows;
using ChartProviders.Common.Interfaces;

namespace ChartProviders.Common.Models
{
    public class TestCase : IDisposable
    {
        private readonly int _index;
        private readonly string _version;
        private readonly ISpeedTest _speedTest;

        private readonly string _testName;
        private readonly TimeSpan _testDuration;
        private readonly TestParameters _testArgs;

        public string Version => _version;
        public int TestNumber => _index;
        public string TestName => $"{_testName} {_testArgs}"; 
        public FrameworkElement TestContent => _speedTest.Element;
        
        public TestCase(int i, string version, string testName, ISpeedTest speedTest, TimeSpan testDuration, TestParameters testArgs)
        {
            _index = i;
            _version = version;
            _speedTest = speedTest;
            _testName = testName;
            _testDuration = testDuration;
            _testArgs = testArgs;
        }

        public void Run(Action<double> completed)
        {
            // Arction added memory garbage collecting 
            GC.Collect();
            GC.WaitForPendingFinalizers(); 
            
            _speedTest.Execute(_testArgs, _testDuration, completed);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _speedTest?.Dispose();
            }
        }
    }
}