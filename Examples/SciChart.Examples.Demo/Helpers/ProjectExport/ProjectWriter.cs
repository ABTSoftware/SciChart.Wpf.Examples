using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;
using SciChart.Charting.Common.Extensions;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Demo.Helpers.ProjectExport
{
    public static class ProjectWriter
    {
        public static readonly string[] AssembliesNames =
        {
            @"SciChart.Core.dll",
            @"SciChart.Data.dll",
            @"SciChart.Charting.dll",
            @"SciChart.Charting3D.dll",
            @"SciChart.Drawing.dll",
            @"SciChart.Charting.DrawingTools.dll",
        };

        public static readonly string ExternalDependencies = @"SciChart.Examples.ExternalDependencies.dll";
        
        public static readonly string ProjectFileName = "ProjectFile.csproj";
        public static readonly string MainWindowFileName = "MainWindow.xaml";
        public static readonly string AssemblyInfoFileName = "AssemblyInfo.cs";
        public static readonly string ClrNamespace = "clr-namespace:";
        public static readonly string ViewModelKey = "ViewModel";
        public static readonly XNamespace DefaultXmlns = "http://schemas.microsoft.com/developer/msbuild/2003";
        public static readonly XNamespace PresentationXmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        public static readonly XNamespace XXmlns = "http://schemas.microsoft.com/winfx/2006/xaml";

        public static string WriteProject(Example example, string selectedPath, string assembliesPath, bool showMessageBox = true)
        {
            var files = new Dictionary<string, string>();

            var assembly = typeof (ProjectWriter).Assembly;

            string[] names = assembly.GetManifestResourceNames();
            var templateFiles = names.Where(x => x.Contains("Templates")).ToList();

            foreach (var templateFile in templateFiles)
            {
                var fileName = GetFileNameFromNs(templateFile);

                using (var s = assembly.GetManifestResourceStream(templateFile))
                using (var sr = new StreamReader(s))
                {
                    files.Add(fileName, sr.ReadToEnd());
                }
            }

            string projectName = "SciChart_" + Regex.Replace(example.Title, @"[^A-Za-z0-9]+", string.Empty);
            files[ProjectFileName] = GenerateProjectFile(files[ProjectFileName], example, projectName, assembliesPath + @"\");
            files.RenameKey(ProjectFileName, projectName + ".csproj");

            files[MainWindowFileName] = GenerateShellFile(files[MainWindowFileName], example).Replace("[ExampleTitle]", example.Title);

            foreach (var codeFile in example.SourceFiles)
            {
                files.Add(codeFile.Key, codeFile.Value);
            }

            WriteProjectFiles(files, selectedPath + projectName + @"\");

            if (showMessageBox)
            {
                MessageBox.Show(string.Format("The {0} example was succesfully exported to {1}", example.Title, selectedPath + projectName), 
                    "Success!");
            }

            return projectName;
        }

        private static string GetFileNameFromNs(string fullName)
        {
            return fullName
                .Replace("SciChart.Examples.Demo.Helpers.ProjectExport.Templates.", string.Empty)
                .Replace(".c.", ".cs.")
                .Replace(@".txt", string.Empty);
        }

        private static string GenerateProjectFile(string projFileSource, Example example, string projectName, string assembliesPath)
        {
            var projXml = XDocument.Parse(projFileSource);

            if (projXml.Root != null)
            {
                //Change RootNamespace and AssemblyName for actual one, in our case - example title.
                foreach (XElement element in projXml.Root.Descendants().Where(x => x.Name.LocalName == "RootNamespace" || x.Name.LocalName == "AssemblyName"))
                {
                    element.SetValue(projectName);
                }

                var elements = projXml.Root.Elements().Where(x => x.Name.LocalName == "ItemGroup").ToList();

                //Add appropriate References
                var el = new XElement(DefaultXmlns + "Reference", new XAttribute("Include", ExternalDependencies.Replace(".dll", string.Empty)));
                el.Add(new XElement(DefaultXmlns + "HintPath", Path.Combine(assembliesPath, ExternalDependencies)));
                elements[0].Add(el);

//                var el2 = new XElement(DefaultXmlns + "Reference", new XAttribute("Include", Interactivity.Replace(".dll", string.Empty)));
//                el2.Add(new XElement(DefaultXmlns + "HintPath", Path.Combine(assembliesPath, Interactivity)));
//                elements[0].Add(el2);

                foreach (var asmName in AssembliesNames)
                {
                    el = new XElement(DefaultXmlns + "Reference", new XAttribute("Include", asmName.Replace(".dll", string.Empty)));
                    el.Add(new XElement(DefaultXmlns + "HintPath", Path.Combine(assembliesPath, asmName)));
                    elements[0].Add(el);
                }

                //Add XElement <Compile> for all .cs files
                var codeFiles = example.SourceFiles.Where(x => x.Key.EndsWith(".cs"));
                foreach (var codeFile in codeFiles)
                {
                    el = new XElement(DefaultXmlns + "Compile", new XAttribute("Include", codeFile.Key));
                    el.Add(new XElement(DefaultXmlns + "DependentUpon", codeFile.Key.Replace(".cs", string.Empty)));
                    elements[1].Add(el);
                }

                //Add XElement <Page> for all .xaml files
                var uiCodeFiles = example.SourceFiles.Where(x => x.Key.EndsWith(".xaml"));
                foreach (var uiFile in uiCodeFiles)
                {
                    el = new XElement(DefaultXmlns + "Page", new XAttribute("Include", uiFile.Key));
                    el.Add(new XElement(DefaultXmlns + "Generator", "MSBuild:Compile"));
                    el.Add(new XElement(DefaultXmlns + "SubType", "Designer"));
                    elements[1].Add(el);
                }
            }

            return projXml.ToString();
        }

        //InjectExampleIntoShell
        private static string GenerateShellFile(string shellFileSource, Example example)
        {
            var sourceFiles = example.SourceFiles;

            var view = DirectoryHelper.GetFileNameFromPath(example.Page.Uri);
            var xamlFile = sourceFiles.Where(pair => pair.Key.EndsWith(".xaml")).FirstOrDefault(x => x.Key == view);

            string fileName;
            var ns = GetExampleNamespace(xamlFile.Value, out fileName);

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

        private static void WriteProjectFiles(Dictionary<string, string> files, string selectedPath)
        {
            Directory.CreateDirectory(selectedPath + @"\");
            Directory.CreateDirectory(selectedPath + @"\Properties\");

            var assemblyInfo = files[AssemblyInfoFileName];
            using (var f = new StreamWriter(selectedPath + @"Properties\" + AssemblyInfoFileName))
            {
                f.Write(assemblyInfo);
            }
            files.Remove(AssemblyInfoFileName);

            foreach (var file in files)
            {
                using (var f = new StreamWriter(selectedPath + file.Key))
                {
                    f.Write(file.Value);
                }
            }
        }
    }
}
