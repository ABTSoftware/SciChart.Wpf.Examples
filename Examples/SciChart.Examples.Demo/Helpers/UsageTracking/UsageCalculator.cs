// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// UsageCalculator.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using SciChart.Examples.Demo.Common;
using SciChart.UI.Bootstrap;

namespace SciChart.Examples.Demo.Helpers.UsageTracking
{
    public interface IUsageCalculator
    {
        Dictionary<string, ExampleUsage> Usages { get; set; }
        Dictionary<string, ExampleRating> Ratings { get; set; }
        ExampleUsage GetUsage(Example example);
        void UpdateUsage(AppPage currentExample);
    }

    [ExportType(typeof(IUsageCalculator), CreateAs.Singleton)]
    public class UsageCalculator : IUsageCalculator
    {
        private ExampleAppPage _exampleAppPage;
        private DateTime _lastNavigationTime;

        public Dictionary<string, ExampleUsage> Usages { get; set; }
        public Dictionary<string, ExampleRating> Ratings { get; set; }

        public UsageCalculator()
        {
            Usages = new Dictionary<string, ExampleUsage>();
            Ratings = new Dictionary<string, ExampleRating>();
        }
        

        public ExampleUsage GetUsage(Example example)
        {
            if (!Usages.ContainsKey(example.Title))
            {
                Usages[example.Title] = new ExampleUsage
                {
                    ExampleID = example.Title,
                    VisitCount = 0,
                    SecondsSpent = 0
                };
            }
            return Usages[example.Title];  
        }

        public void UpdateUsage(AppPage newExample)
        {
            var timeNow = DateTime.UtcNow;

            if (_exampleAppPage != null)
            {
                if (Usages.ContainsKey(_exampleAppPage.Title))
                {
                    Usages[_exampleAppPage.Title].VisitCount ++;
                    Usages[_exampleAppPage.Title].SecondsSpent += (int)(timeNow - _lastNavigationTime).TotalSeconds;
                }
                else
                {
                    Usages[_exampleAppPage.Title] = new ExampleUsage
                    {
                        ExampleID = _exampleAppPage.Title,
                        VisitCount =  1,
                        SecondsSpent = (int)(timeNow - _lastNavigationTime).TotalSeconds,
                    };
                }
            }

            _lastNavigationTime = timeNow;
            _exampleAppPage = newExample as ExampleAppPage;
        }

    }
}