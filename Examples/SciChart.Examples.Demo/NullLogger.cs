// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// NullLogger.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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