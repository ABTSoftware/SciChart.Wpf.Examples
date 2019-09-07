using System.Runtime.InteropServices;

namespace Fifo100MillionPointsDemo.HelperClasses
{
    public class SysInfo
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

        public static long GetRamGb()
        {
            GetPhysicallyInstalledSystemMemory(out var memKb);
            return(memKb / 1024 / 1024);
        }
    }
}