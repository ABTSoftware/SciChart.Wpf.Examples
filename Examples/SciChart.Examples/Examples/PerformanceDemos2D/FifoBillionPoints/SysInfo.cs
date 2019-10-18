using System.Runtime.InteropServices;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class SysInfo
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

        public static long GetRamGb()
        {
            GetPhysicallyInstalledSystemMemory(out var memKb);
            return(memKb / 1024 / 1024);
        }
    }
}