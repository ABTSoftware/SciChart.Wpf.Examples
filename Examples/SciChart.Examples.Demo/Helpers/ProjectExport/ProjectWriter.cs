using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.Demo.Helpers.ProjectExport
{
    public static class ProjectWriter
    {
        public static readonly string[] AssembliesNames =
        {
            "SciChart.Core.dll",
            "SciChart.Data.dll",
            "SciChart.Charting.dll",
            "SciChart.Charting3D.dll",
            "SciChart.Drawing.dll",
            "SciChart.Charting.DrawingTools.dll",
            "SciChart.Examples.ExternalDependencies.dll"
        };

        public static readonly string[] AssembliesNuGetPackages =
        {
            "SciChart",
            "SciChart3D",
            "SciChart.DrawingTools",
            "SciChart.ExternalDependencies"
        };

        public static readonly string[] ExamplesNuGetPackages =
        {
            //ExampleTitle;PackageName;PackageVersion
            "AudioAnalyzerDemo;NAudio;1.10.0",
            "VitalSignsMonitorDemo;System.Reactive;4.4.1"
        };

        public static readonly string ExampleTheme = "Navy";

        public static readonly string AssemblyInfoFileName = "AssemblyInfo.cs";
        public static readonly string SolutionFileName = "SolutionFile.sln";
        public static readonly string ProjectFileName = "ProjectFile.csproj";

        public static readonly string ApplicationFileName = "App.xaml";
        public static readonly string MainWindowFileName = "MainWindow.xaml";
        public static readonly string ExampleResourcesFileName = "ExampleResources.xaml";

        public static readonly string ClrNamespace = "clr-namespace:";
        public static readonly string ViewModelKey = "ViewModel";

        public static readonly XNamespace PresentationXmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        public static readonly XNamespace XXmlns = "http://schemas.microsoft.com/winfx/2006/xaml";

        public static int VersionMajor => int.Parse(SciChartSurface.VersionInfo?.Split('.')[0].Trim('v') ?? "0");

        public static string WriteProject(Example example, string selectedPath, string assembliesPath, bool useLibsFromFolder)
        {
            var files = new Dictionary<string, string>();
            var assembly = typeof(ProjectWriter).Assembly;

            var names = assembly.GetManifestResourceNames();
            var templateFiles = names.Where(x => x.Contains("Templates")).ToList();

            foreach (var templateFile in templateFiles)
            {
                var fileName = GetFileNameFromNs(templateFile);

                using (var s = assembly.GetManifestResourceStream(templateFile))
                {
                    if (s == null) break;

                    using (var sr = new StreamReader(s))
                    {
                        files.Add(fileName, sr.ReadToEnd());
                    }
                }
            }

            var projectName = "SciChart_" + Regex.Replace(example.Title, @"[^A-Za-z0-9]+", string.Empty);

            files[ProjectFileName] = GenerateProjectFile(files[ProjectFileName], example, assembliesPath, useLibsFromFolder);
            files[SolutionFileName] = GenerateSolutionFile(files[SolutionFileName], projectName);

            files.RenameKey(ProjectFileName, projectName + ".csproj");
            files.RenameKey(SolutionFileName, projectName + ".sln");

            files[ApplicationFileName] = GenerateApplicationFile(files[ApplicationFileName], ExampleTheme);
            files[MainWindowFileName] = GenerateShellFile(files[MainWindowFileName], example).Replace("[EXAMPLE_TITLE]", example.Title);

            foreach (var codeFile in example.SourceFiles)
            {
                files.Add(codeFile.Key, codeFile.Value);
            }

            if (!files.ContainsKey(ExampleResourcesFileName))
            {
                files.Add(ExampleResourcesFileName, GenerateExampleResourcesFile(ExampleTheme));
            }

            var exportPath = Path.Combine(selectedPath, projectName);
            var isExported = WriteProjectFiles(files, exportPath);
            
            return isExported ? projectName : null;
        }

        private static string GetFileNameFromNs(string fullName)
        {
            return fullName
                .Replace("SciChart.Examples.Demo.Helpers.ProjectExport.Templates.", string.Empty)
                .Replace(".c.", ".cs.")
                .Replace(".txt", string.Empty);
        }

        private static string GenerateSolutionFile(string file, string projectName)
        {
            return file.Replace("[PROJECT_NAME]", projectName);
        }

        private static string GenerateApplicationFile(string file, string themeName)
        {
            return file.Replace("[THEME_NAME]", themeName);
        }

        private static string GenerateExampleResourcesFile(string themeName)
        {
            var dictionary = ThemeLoader.LoadThemeFile(themeName);
            var pattern = "<ResourceDictionary\\.MergedDictionaries>[\\s\\S]*?<\\/ResourceDictionary\\.MergedDictionaries>";

            return Regex.Replace(dictionary, pattern, string.Empty);
        }

        private static string GenerateProjectFile(string projFileSource, Example example, string assembliesPath, bool useLibsFromFolder)
        {
            var projXml = XDocument.Parse(projFileSource);
            if (projXml.Root != null)
            {
                var elements = projXml.Root.Elements().Where(x => x.Name.LocalName == "ItemGroup").ToList();
                if (elements.Count == 3)
                {
                    if (useLibsFromFolder)
                    {
                        // Add assembly references
                        foreach (var asmName in AssembliesNames)
                        {
                            var el = new XElement("Reference", new XAttribute("Include", asmName.Replace(".dll", string.Empty)));
                            el.Add(new XElement("HintPath", Path.Combine(assembliesPath, asmName)));                          
                            elements[0].Add(el);
                        }
                    }
                    else
                    {
                        // Add assembly NuGet packages
                        foreach (var asmPackageName in AssembliesNuGetPackages)
                        {
                            var el = new XElement("PackageReference",
                                new XAttribute("Include", asmPackageName),
                                new XAttribute("Version", $"{VersionMajor}.*-*"));

                            elements[1].Add(el);
                        }  
                    }

                    // Add package references for specific example NuGet packages
                    var exampleTitle = Regex.Replace(example.Title, @"\s", string.Empty);
                    var examplePackages = ExamplesNuGetPackages.Where(p => p.StartsWith(exampleTitle));
                    if (examplePackages.Any())
                    {
                        foreach (var package in examplePackages)
                        {
                            // ExampleTitle;PackageName;PackageVersion
                            var packageAttr = package.Split(';');
                            if (packageAttr.Length == 3)
                            {
                                var el = new XElement("PackageReference",
                                    new XAttribute("Include", packageAttr[1]),
                                    new XAttribute("Version", packageAttr[2]));

                                elements[1].Add(el);
                            }
                        }
                    }

                    var targetFramework =
#if NETFRAMEWORK
                        "net462"
#elif NET
                        "net6.0-windows"
#elif NETCOREAPP3_1
                        "netcoreapp3.1"
#else
                        "net6.0-windows"
#endif
                    ;

                    return projXml.ToString().Replace("[PROJECT_TARGET]", targetFramework);
                }
            }

            return projFileSource;
        }

        private static string GenerateShellFile(string shellFileSource, Example example)
        {
            var sourceFiles = example.SourceFiles;

            var view = DirectoryHelper.GetFileNameFromPath(example.Page.Uri);
            var xamlFile = sourceFiles.Where(pair => pair.Key.EndsWith(".xaml")).FirstOrDefault(x => x.Key == view);

            var ns = GetExampleNamespace(xamlFile.Value, out string fileName);
            var xml = XDocument.Parse(shellFileSource);

            if (xml.Root != null)
            {
                //xmlns
                xml.Root.Add(new XAttribute(XNamespace.Xmlns + "example", ClrNamespace + ns));

                var userControlElement = new XElement((XNamespace)(ClrNamespace + ns) + fileName);

                //ViewModel to Resources
                if (example.Page.ViewModel != null)
                {
                    var el = new XElement(PresentationXmlns + "Window.Resources");
                    el.Add(new XElement((XNamespace)(ClrNamespace + ns) + example.Page.ViewModel.GetType().Name,
                        new XAttribute(XXmlns + "Key", ViewModelKey)));

                    xml.Root.Add(el);

                    //Add DataContext to UserControl is needed
                    userControlElement.Add(new XAttribute("DataContext", GetStaticResource(ViewModelKey)));
                }

                xml.Root.Add(userControlElement);
            }

            return xml.ToString();
        }

        private static string GetExampleNamespace(string xamlFile, out string xamlFileName)
        {
            var xml = XDocument.Parse(xamlFile);
            if (xml.Root != null)
            {
                var xClassAttribute = xml.Root.Attributes().FirstOrDefault(x => x.Name.LocalName == "Class");
                if (xClassAttribute != null)
                {
                    var classNs = xClassAttribute.Value;
                    var index = classNs.LastIndexOf('.');
                    if (index > 0)
                    {
                        xamlFileName = classNs.Substring(index + 1, classNs.Length - index - 1);
                        return classNs.Substring(0, index);
                    }
                }
            }
            xamlFileName = null;
            return null;
        }

        private static string GetStaticResource(string resourceKey)
        {
            return string.Format("{{StaticResource {0}}}", resourceKey);
        }

        private static bool WriteProjectFiles(Dictionary<string, string> files, string selectedPath)
        {
            try
            {
                Directory.CreateDirectory(selectedPath);

                foreach (var file in files)
                {
                    using (var sw = new StreamWriter(Path.Combine(selectedPath, file.Key)))
                    {
                        sw.Write(file.Value);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            } 
        }
    }
}