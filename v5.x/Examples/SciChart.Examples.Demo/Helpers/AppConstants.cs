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
