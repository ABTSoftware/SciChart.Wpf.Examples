// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// TimeFrame.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Globalization;
using System.Linq;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class TimeFrame : StrongTyped<string>
    {        
        public TimeFrame(string value, string displayname) : base(value)
        {
            Displayname = displayname;
        }

        public static readonly TimeFrame Daily = new TimeFrame("Daily", "Daily");
        public static readonly TimeFrame Hourly = new TimeFrame("Hourly", "1 Hour");
        public static readonly TimeFrame Minute15 = new TimeFrame("Minute15", "15 Minutes");
        public static readonly TimeFrame Minute5 = new TimeFrame("Minute5", "5 Minutes");

        public string Displayname { get; private set; }

        public static TimeFrame Parse(string input)
        {
            return new[] {Minute5, Minute15, Hourly, Daily}.Single(x => x.Value.ToUpper(CultureInfo.InvariantCulture) == input.ToUpper(CultureInfo.InvariantCulture));
        }
    }
}