// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// AppConstants.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
namespace SciChart.Examples.Demo.Helpers
{
    public static class AppConstants
    {
        public const string AssemblyName = "SciChart.Examples.";
        public const string DemoAssemblyName = "SciChart.Examples.Demo.";

        public const string OldAppPath = "/Abt.Controls.SciChart.Example;component/";

        public const string ComponentPath =
#if SILVERLIGHT
            "/SciChart.Examples.SL;component/"
#else
            "SciChart.Examples;component/"
#endif 
            ; 

        public const string DemoComponentPath =
#if SILVERLIGHT
            "/SciChart.Examples.SL.Demo;component/"
#else
            "SciChart.Examples.Demo;component/"
#endif 
            ;
    }
}
