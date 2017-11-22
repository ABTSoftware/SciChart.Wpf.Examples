using System;

namespace SciChart.Sandbox
{
    public class TestCaseAttribute : Attribute
    {
        public string TestCaseName { get; private set; }

        public TestCaseAttribute(string testCaseName)
        {
            TestCaseName = testCaseName;
        }
    }
}