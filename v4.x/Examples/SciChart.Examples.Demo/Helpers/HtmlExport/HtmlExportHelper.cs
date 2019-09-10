using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using SciChart.Core.Utility;
using SciChart.Drawing.Extensions;
using SciChart.Examples.Demo.Helpers.Navigation;
using SciChart.Examples.Demo.Helpers.ProjectExport;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Examples.Demo.Helpers.HtmlExport
{
    public static class HtmlExportHelper
    {
        public const string ExportHtmlParameter = "/exporthtml";
        public const string FolderName = "ExportedExamples";

        private const string IndexTemplateFileName = "ExportedExampleIndex.html";
        private const string TemplateFileName = "ExportedExampleTemplate.html";
        private const string TitleTag = "[Title]";
        private const string CategoryTag = "[Category]";
        private const string GroupTag = "[Group]";
        private const string ImageTag = "[Image]";
        private const string DescriptionTag = "[Description]";
        private const string ExamplePath = "[ExamplePath]";
        private const string SourceCodeTag = "[SourceCode]";

        private static readonly string DefaultExportPath;
        private static string ExportPath { get; set; }

        static HtmlExportHelper()
        {
            DefaultExportPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        private static bool GetPathForExport()
        {
            var basePath = DirectoryHelper.GetPathForExport(DefaultExportPath);
            if(string.IsNullOrEmpty(basePath)) return false;

            ExportPath =  basePath + "\\" + FolderName + "\\";
            CreateExampleFolder(ExportPath);

            return true;
        }

        public static void ExportExamplesToHtml(IModule module)
        {
            var enumerator = module.Examples.Select(x => x.Value).GetEnumerator();
            
            if(!GetPathForExport()) return;
            
            Action<UserControl, AppPage> action = null;
            action = (control, page) =>
            {
                RoutedEventHandler controlOnLoaded = null;
                controlOnLoaded = (sender, args) =>
                {
                    TimedMethod.Invoke(() =>
                    {
                        ExportExampleToHtml(enumerator.Current, false);

                        if (enumerator.MoveNext())
                        {
                            Navigator.Instance.Navigate(enumerator.Current);
                            ServiceLocator.Container.Resolve<IModule>().CurrentExample = enumerator.Current;
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

            ExportIndexToHtml(module);
        }

        private static void ExportIndexToHtml(IModule module)
        {
            var assembly = typeof(HtmlExportHelper).Assembly;

            string[] names = assembly.GetManifestResourceNames();

            var templateFile = names.SingleOrDefault(x => x.Contains(IndexTemplateFileName));

            using (var s = assembly.GetManifestResourceStream(templateFile))
            using (var sr = new StreamReader(s))
            {
                string lines = sr.ReadToEnd();

                StringBuilder sb = new StringBuilder();
                foreach (var categoryGroup in module.Examples.Values.GroupBy(x => x.TopLevelCategory))
                {
                    sb.Append("<h2>").Append(categoryGroup.Key).Append("</h2>").Append(Environment.NewLine);

                    foreach (var exampleGroup in categoryGroup.GroupBy(x => x.Group))
                    {
                        sb.Append("<h4>").Append(exampleGroup.Key).Append("</h4>").Append(Environment.NewLine);
                        sb.Append("<ul>").Append(Environment.NewLine);
                        foreach (var example in exampleGroup)
                        {
                            var fileName = string.Format("wpf-{0}chart-example-{1}",
                                example.TopLevelCategory.ToUpper().Contains("3D") || example.Title.ToUpper().Contains("3D") ? "3d-" : string.Empty,
                                example.Title.ToLower().Replace(" ", "-"));
                            sb.Append("<li>").Append("<a href=\"").Append(fileName).Append("\">").Append(example.Title).Append("</a></li>");
                        }
                        sb.Append("</ul>").Append(Environment.NewLine);
                    }                    
                }
                lines = lines.Replace("[Index]", sb.ToString());

                File.WriteAllText(Path.Combine(ExportPath, "wpf-chart-examples.html"), lines);
            }
        }

        public static void ExportExampleToHtml(Example example, bool isUniqueExport = true)
        {
            if (isUniqueExport)
            {
                if (!GetPathForExport())
                {
                    MessageBox.Show("Bad path. Please, try export once more.");
                    return;
                }
            }

            var assembly = typeof (HtmlExportHelper).Assembly;

            string[] names = assembly.GetManifestResourceNames();

            var templateFile = names.SingleOrDefault(x => x.Contains(TemplateFileName));

            using (var s = assembly.GetManifestResourceStream(templateFile))
            using (var sr = new StreamReader(s))
            {
                string lines = sr.ReadToEnd();

                var exampleFolderPath = ExportPath;//string.Format("{0}{1}\\", ExportPath, example.Title);
                //CreateExampleFolder(exampleFolderPath);

                PrintMainWindow("scichart-wpf-chart-example-" + example.Title.ToLower().Replace(" ", "-"), exampleFolderPath);
                
                lines = ReplaceTagsWithExample(lines, example);
                var fileName = string.Format("wpf-{0}chart-example-{1}.html",                    
                    example.TopLevelCategory.ToUpper().Contains("3D") || example.Title.ToUpper().Contains("3D") ? "3d-" : string.Empty,
                    example.Title.ToLower().Replace(" ", "-"));

                File.WriteAllText(Path.Combine(ExportPath, fileName), lines);
            }
            
        }

        private static void PrintMainWindow(string title, string path)
        {
            var frameworkElement = (FrameworkElement)Application.Current.MainWindow.Content;
            var bitmap = frameworkElement.RenderToBitmap((int)frameworkElement.ActualWidth, (int)frameworkElement.ActualHeight);
            
            SaveBitmap(path + title + ".png", bitmap);
            
            //Optional - store image to common folder
            //SaveBitmap(ExportPath + title + ".png", bitmap);
        }

        private static void SaveBitmap(string filename, WriteableBitmap image5)
        {
            if (filename != string.Empty)
            {
                using (var s = new FileStream(filename, FileMode.Create))
                {
                    var encoder5 = new PngBitmapEncoder();
                    encoder5.Frames.Add(BitmapFrame.Create(image5));
                    encoder5.Save(s);
                    s.Close();
                }
            }
        }

        private static void CreateExampleFolder(string exportPath)
        {
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }
        }

        private static string ReplaceTagsWithExample(string lines, Example example)
        {           
            lines = lines.Replace(TitleTag, example.Title);
            lines = lines.Replace(ImageTag, "scichart-wpf-chart-example-" + example.Title.ToLower().Replace(" ", "-") + ".png");
            lines = lines.Replace(DescriptionTag, example.HtmlDescription);
            lines = lines.Replace(ExamplePath, string.Format("{0} > {1} > {2}", example.TopLevelCategory, example.Group, example.Title));
            lines = lines.Replace(CategoryTag, example.TopLevelCategory);
            lines = lines.Replace(GroupTag, example.Group);

            var sb = new StringBuilder();            
            
            foreach (var pair in example.SourceFiles)
            {
                sb.AppendLine(string.Format("<h4>{0}</h4>", pair.Key));
                var code = pair.Value.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
                if (pair.Key.ToUpper().EndsWith("XAML"))
                {
                    sb.AppendLine(string.Format(@"<pre class=""brush: xml; gutter: true; first-line: 1; highlight: []; html-script: false"">{0}</pre>", code));
                }
                else
                {
                    sb.AppendLine(string.Format(@"<pre class=""brush: csharp; gutter: true; first-line: 1; highlight: []; html-script: false"">{0}</pre>", code));   
                }                    
            }
            lines = lines.Replace(SourceCodeTag, sb.ToString());

            return lines;
        }
    }
}