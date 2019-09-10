// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ExampleManager.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SciChart.Examples
{
    public static class ExampleManager
    {
        public static IDictionary<string, string> GetAllExampleDefinitions()
        {
            var dictionary = new Dictionary<string, string>();
            var assembly = typeof(ExampleManager).Assembly;

            var exampleNames = assembly.GetManifestResourceNames().Where(x => x.Contains("ExampleSourceFiles")).ToList();
            exampleNames.Sort();

            foreach (var example in exampleNames)
            {
                using (var s = assembly.GetManifestResourceStream(example))
                using (var sr = new StreamReader(s))
                {
                    dictionary.Add(example.Replace("SciChart.Example.Resources.ExampleDefinitions.", string.Empty),
                                   sr.ReadToEnd());
                }
            }
            return dictionary;
        }
    }
}