using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SciChart.Charting.Visuals;
using SciChart.Core.Utility;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.Examples.Demo.Helpers.ProjectExport;

namespace SciChart.Examples.Demo.ViewModels
{
    public class ExportExampleHelper
    {
        private static string _scriptPath;
        private static string _exportPath;

        public const string FolderName = "ExportedSolutions";
        private const string RegistryKeyString = @"Software\SciChart Ltd\SciChart";

        static ExportExampleHelper()
        {
            DefaultExportPath = AppDomain.CurrentDomain.BaseDirectory + FolderName + "\\";

            AssemblyVersion = GetSciChartVersion();
        }

        public static string DefaultExportPath { get; }

        public static string AssemblyVersion { get; }

        public static string ScriptPath
        {
            get
            {
                if (_scriptPath == null)
                {
                    return Path.Combine(_exportPath, "CompileExamples.bat"); 
                }

                return _scriptPath;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _scriptPath = value;
                }
            }           
        }

        public static void ExportExamplesToSolutions(IModule module)
        {
            var enumerator = module.Examples.Select(x => x.Value).GetEnumerator();
            var basePath = DirectoryHelper.GetPathForExport(DefaultExportPath);

            if (string.IsNullOrEmpty(basePath)) return;

            _exportPath = basePath + "\\" + FolderName + "\\";

            try
            {
                if (File.Exists(ScriptPath))
                    File.Delete(ScriptPath);

                if (Directory.Exists(_exportPath))
                    Directory.Delete(_exportPath, true);

                if (!Directory.Exists(_exportPath))
                    Directory.CreateDirectory(_exportPath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("A permissions error occurred when deleting or creating example export paths. Please check you are not trying to export in a restricted directory such as C: or Program Files", ex);
            }

            string lastGroup = null;
            Action<UserControl, AppPage> action = null;
            action = (control, page) =>
            {
                RoutedEventHandler controlOnLoaded = null;
                controlOnLoaded = (sender, args) =>
                {
                    TimedMethod.Invoke(() =>
                    {
                        ExportExampleToSolution(ref lastGroup, enumerator.Current);                        

                        if (enumerator.MoveNext())
                        {
                            Navigator.Instance.Navigate(enumerator.Current);
                        }
                        else
                        {
                            Navigator.Instance.AfterNavigation -= action;
                        }

                        control.Loaded -= controlOnLoaded;
                    }).After(1000).Go();
                };
                control.Loaded += controlOnLoaded;
            };

            Navigator.Instance.AfterNavigation += action;

            if (enumerator.MoveNext())
            {
                Navigator.Instance.Navigate(enumerator.Current);
            }
        }

        private static void ExportExampleToSolution(ref string lastGroup, Example current)
        {
            string projectName = ProjectWriter.WriteProject(
                current, _exportPath + @"\", TryAutomaticallyFindAssemblies(), false);

            if (!File.Exists(ScriptPath))
            {
                using (var fs = File.CreateText(ScriptPath))
                {
                    // Write the header
                    fs.WriteLine("REM Compile and run all exported SciChart examples for testing");
                    fs.WriteLine("@echo OFF");                    
                    fs.WriteLine("");
                    fs.Close();
                }                
            }

            if (lastGroup != null && current.Group != lastGroup)
            {
                using (var fs = File.AppendText(ScriptPath))
                {
                    fs.WriteLine("@echo Finished Example Group " + lastGroup + ". Press any key to continue...");
                    fs.WriteLine("pause(0)");
                    fs.WriteLine("");
                }
            }
            lastGroup = current.Group;

            using (var fs = File.AppendText(ScriptPath))
            {
                fs.WriteLine("@echo Building " + projectName);
                fs.WriteLine(@"IF NOT EXIST ""C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin"" @echo VisualStudio folder not exists with the following path: ""C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin"" ");
                fs.WriteLine(@"call ""C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"" /p:Configuration=""Debug"" ""{0}/{0}.csproj"" /p:WarningLevel=0", projectName);
                fs.WriteLine("if ERRORLEVEL 1 (");
                fs.WriteLine("   @echo - Example {0} Failed to compile >> errorlog.txt", projectName);
                fs.WriteLine(") else (");
                fs.WriteLine(@"   start """" ""{0}/bin/Debug/{0}.exe""", projectName);
                fs.WriteLine(")");
                fs.WriteLine("");
            }
        }

        public static string TryAutomaticallyFindAssemblies()
        {
            var path = GetExeecutedAssemblyPath();
            if (!string.IsNullOrEmpty(path))
            {
                var folderPath = GetFolderFromFullPath(path);
                var isAssembliesValid = SearchForCoreAssemblies(folderPath);

                return isAssembliesValid ? folderPath : GetAssemblyPathFromRegistry();
            }
            return null;
        }

        private static string GetExeecutedAssemblyPath()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        private static string GetFolderFromFullPath(string path)
        {
            var index = path.LastIndexOf(@"\", StringComparison.InvariantCulture);
            var folderPath = path.Substring(0, index + 1);

            return folderPath;
        }

        public static bool SearchForCoreAssemblies(string folderPath)
        {
            bool result = true;
            foreach (var asmName in ProjectWriter.AssembliesNames)
            {
                result &= IsAssemblyExist(folderPath, asmName) &&
                          IsAssemblyVersionMatch(folderPath, asmName);
            }
            result &= IsAssemblyExist(folderPath, ProjectWriter.ExternalDependencies);

            return result;
        }

        public static bool IsAssemblyExist(string folderPath, string assemblyName)
        {
            string fullPath = Path.Combine(folderPath, assemblyName);

            bool isExists = File.Exists(fullPath);

            return isExists;
        }

        public static bool IsAssemblyVersionMatch(string folderPath, string assemblyName)
        {
            string fullPath = Path.Combine(folderPath, assemblyName);

            bool isMatch = FileVersionInfo.GetVersionInfo(fullPath).FileVersion == AssemblyVersion;

            return isMatch;
        }

        public static string GetAssemblyPathFromRegistry()
        {
            using (var registryKey = Registry.CurrentUser.OpenSubKey(RegistryKeyString))
            {
                if (registryKey != null)
                {
                    var assemblyPathFromRegistry = Path.Combine((string)registryKey.GetValue("Path"), @"Lib\net40\");
                    
                    return assemblyPathFromRegistry;
                }
            }

            return null;
        }

        public static string GetSciChartVersion()
        {
            var assemblyName = new AssemblyName(typeof(SciChartSurface).Assembly.FullName);

            return assemblyName.Version.ToString();
        }    
    }
}