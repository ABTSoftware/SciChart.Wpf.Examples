using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class PowerManager : IPowerManager
    {
        /// <summary>
        /// Indicates that almost no power savings measures will be used.
        /// </summary>
        internal readonly PowerPlan MaximumPerformance;

        /// <summary>
        /// Indicates that fairly aggressive power savings measures will be used.
        /// </summary>
        internal readonly PowerPlan Balanced;

        /// <summary>
        /// Indicates that very aggressive power savings measures will be used to help
        /// stretch battery life.                                                     
        /// </summary>
        internal readonly PowerPlan PowerSourceOptimized;

        public PowerManager()
        {
            // See GUID values in WinNT.h.
            MaximumPerformance = NewPlan("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
            Balanced = NewPlan("381b4222-f694-41f0-9685-ff5bb260df2e");
            PowerSourceOptimized = NewPlan("a1841308-3541-4fab-bc81-f71556f20b4a");            
        }

        private PowerPlan NewPlan(string guidString)
        {
            Guid guid = new Guid(guidString);
            return new PowerPlan(GetPowerPlanName(guid), guid);
        }

        public void SetActive(PowerPlan plan)
        {
            PowerSetActiveScheme(IntPtr.Zero, ref plan.Guid);
        }

        /// <returns>
        /// All supported power plans.
        /// </returns>
        public List<PowerPlan> GetPlans()
        {
            return new List<PowerPlan>(new PowerPlan[] {
                MaximumPerformance,
                Balanced,
                PowerSourceOptimized
            });
        }

        private Guid GetActiveGuid()
        {
            Guid ActiveScheme = Guid.Empty;
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            if (PowerGetActiveScheme((IntPtr)null, out ptr) == 0)
            {
                ActiveScheme = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
                if (ptr != null)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            return ActiveScheme;
        }

        public PowerPlan GetCurrentPlan()
        {
            Guid guid = GetActiveGuid();
            var foundPlan = GetPlans().Find(p => (p.Guid == guid));
            return foundPlan ?? new PowerPlan("Unknown", guid);
        }

        private static string GetPowerPlanName(Guid guid)
        {
            string name = string.Empty;
            IntPtr lpszName = (IntPtr)null;
            uint dwSize = 0;

            PowerReadFriendlyName((IntPtr)null, ref guid, (IntPtr)null, (IntPtr)null, lpszName, ref dwSize);
            if (dwSize > 0)
            {
                lpszName = Marshal.AllocHGlobal((int)dwSize);
                if (0 == PowerReadFriendlyName((IntPtr)null, ref guid, (IntPtr)null, (IntPtr)null, lpszName, ref dwSize))
                {
                    name = Marshal.PtrToStringUni(lpszName);
                }
                if (lpszName != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpszName);
            }

            return name;
        }

        /// <summary>
        /// Opens Power Options section of the Control Panel.
        /// </summary>
        public void OpenControlPanel()
        {
            var root = Environment.GetEnvironmentVariable("SystemRoot");
            Process.Start(root + "\\system32\\control.exe", "/name Microsoft.PowerOptions");
        }

        #region DLL imports

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern int GetSystemDefaultLCID();

        [DllImportAttribute("powrprof.dll", EntryPoint = "PowerSetActiveScheme")]
        public static extern uint PowerSetActiveScheme(IntPtr UserPowerKey, ref Guid ActivePolicyGuid);

        [DllImportAttribute("powrprof.dll", EntryPoint = "PowerGetActiveScheme")]
        public static extern uint PowerGetActiveScheme(IntPtr UserPowerKey, out IntPtr ActivePolicyGuid);

        [DllImportAttribute("powrprof.dll", EntryPoint = "PowerReadFriendlyName")]
        public static extern uint PowerReadFriendlyName(IntPtr RootPowerKey, ref Guid SchemeGuid, IntPtr SubGroupOfPowerSettingsGuid, IntPtr PowerSettingGuid, IntPtr Buffer, ref uint BufferSize);

        #endregion
    }
}