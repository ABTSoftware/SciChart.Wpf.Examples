// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ThemeLoader.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SciChart.Examples
{
    public class ThemeLoader
    {
        public static string LoadThemeFile(string themeName)
        {
            Assembly assembly = typeof(ThemeLoader).Assembly;

            var names = assembly.GetManifestResourceNames();
            var find = $"SciChart.Examples.Themes.{themeName}.xaml";
            var file = names.FirstOrDefault(x => x.EndsWith(find));

            if (file == null)
                throw new Exception(string.Format("Unable to find the example theme resource {0}", find));

            using (var s = assembly.GetManifestResourceStream(file))
            using (var sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }
    }
}