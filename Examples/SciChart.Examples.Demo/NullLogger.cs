using System;
using SciChart.UI.Bootstrap.Utility;

namespace SciChart.Examples.Demo
{
    public class ConsoleLogger : ILogFacade
    {
        public void DebugFormat(string format, params object[] args)
        {
            Console.WriteLine("DEBUG: " + string.Format(format, args));
        }

        public void InfoFormat(string format, params object[] args)
        {
            Console.WriteLine("INFO: " + string.Format(format, args));
        }

        public void Error(Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
        }

        public void Debug(string str)
        {
            Console.WriteLine("DEBUG: " + str);
        }

        public void Error(string message, Exception ex)
        {
            Console.WriteLine("ERROR: " + message + ", " + ex.Message);
        }
    }
}