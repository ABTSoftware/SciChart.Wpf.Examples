using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace WPFChartPerformanceBenchmark
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Assembly[] _chartProviderDlls;

        public static App Instance
        {
            get
            {
                return Application.Current as App;
            }
        }

        public App()
        {            
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                lock (typeof (App))
                {
                    if (_chartProviderDlls == null)
                    {
                        var exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                        _chartProviderDlls = new DirectoryInfo(exeFolder)
                            .GetFiles("*.dll", SearchOption.AllDirectories)
                            .Select(x => x.FullName)
                            .Distinct().Select(Assembly.LoadFile)
                            .ToArray();
                    }
                }

                var asm = _chartProviderDlls.FirstOrDefault(dll => dll.FullName.Equals(args.Name));

                Debug.WriteLine("Resolving {0}, {1}", args.Name, asm == null ? "NOT FOUND" : "Found " + asm.FullName);

                return asm;
            };
        }

    }
}
