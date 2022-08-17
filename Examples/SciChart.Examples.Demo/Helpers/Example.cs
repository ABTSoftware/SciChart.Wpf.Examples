// *************************************************************************************
// SCICHART © Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
// Support: support@scichart.com
//  
// Example.cs is part of SciChart Examples, a High Performance WPF & Silverlight Chart. 
// For full terms and conditions of the SciChart license, see http://www.scichart.com/scichart-eula/
//  
// SciChart Examples source code is provided free of charge on an "As-Is" basis to support
// and provide examples of how to use the SciChart component. You bear the risk of using it. 
// The authors give no express warranties, guarantees or conditions. You may have additional 
// consumer rights under your local laws which this license cannot change. To the extent 
// permitted under your local laws, the contributors exclude the implied warranties of 
// merchantability, fitness for a particular purpose and non-infringement. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using SciChart.Examples.Demo.Common;

namespace SciChart.Examples.Demo.Helpers
{
    public sealed class Example : ISelectable
    {
        private readonly List<string> _sourceFilePaths;
        private readonly Dictionary<string, string> _sourceFiles = new Dictionary<string, string>();
        private bool _isLoaded;
        private static readonly Regex _urlRegex = new Regex(@"\[url='(.*?)'?\](.*?)\[\/url\]");        

        public Example(AppPage page, ExampleDefinition exDefinition)
        {
            Page = page;
            Title = exDefinition.Title;
            FormattedDescription = ParseFormattedDescription(exDefinition.Description);
            HtmlDescription = ParseHtmlDescription(exDefinition.Description);
            Description = SimplifyDescription(exDefinition.Description);
            ToolTipDescription = exDefinition.ToolTipDescription;            
            ExampleImagePath = exDefinition.ImagePath;
            Group = exDefinition.ChartGroup;
            TopLevelCategory = exDefinition.ExampleCategory;
            IconPath = exDefinition.IconPath;
            Features = exDefinition.Features;
            GithubExampleUrl = exDefinition.GithubUrl;
            _sourceFilePaths = exDefinition.CodeFiles;
        }

        private string ParseHtmlDescription(string description)
        {
            if (string.IsNullOrEmpty(description)) return string.Empty;

            var formatted = description
                .Trim('\n', ' ')
                .Replace("\r\n", "\n")
                .Replace("\n", "<br/>")
                .Replace("[b][i]", "<strong><em>")
                .Replace("[i][b]", "<strong><em>")
                .Replace("[b]", "<strong>")
                .Replace("[i]", "<em>")
                .Replace("[/b][/i]", "</em></strong>")
                .Replace("[/i][/b]", "</em></strong>")
                .Replace("[/b]", "</strong>")
                .Replace("[/i]", "</em>")
                .Replace("[gt]", "&gt;")
                .Replace("[lt]", "&lt;");

            const string replace = "<a href=\"$1\" target=\"_blank\">$2</a>";

            formatted = _urlRegex.Replace(formatted, replace);

            return formatted;
        }

        private string ParseFormattedDescription(string formattedDescription)
        {
            if (string.IsNullOrEmpty(formattedDescription)) return string.Empty;

            var formatted = formattedDescription
                .Trim('\n', ' ')
                .Replace("\r\n", "\n")
                .Replace("\n", "<LineBreak/>")
                .Replace("[b][i]", "<Run FontWeight=\"Bold\" FontStyle=\"Italic\">")
                .Replace("[i][b]", "<Run FontWeight=\"Bold\" FontStyle=\"Italic\">")
                .Replace("[b]", "<Run FontWeight=\"Bold\">")
                .Replace("[i]", "<Run FontStyle=\"Italic\">")
                .Replace("[/b][/i]", "</Run>")
                .Replace("[/i][/b]", "</Run>")
                .Replace("[/b]", "</Run>")
                .Replace("[/i]", "</Run>")
                .Replace("[gt]", "&gt;")
                .Replace("[lt]", "&lt;");

            const string replace = "<Hyperlink Style=\"{DynamicResource HyperlinkStyle}\" NavigateUri=\"$1\"><Run>$2</Run></Hyperlink>";

            formatted = _urlRegex.Replace(formatted, replace);

            return formatted;
        }

        private string SimplifyDescription(string formattedDescription)
        {
            if (string.IsNullOrEmpty(formattedDescription)) return string.Empty;

            var formatted = formattedDescription
                .Trim('\n', ' ')
                .Replace("\r\n", "\n")
                .Replace("\n", " ")
                .Replace("    ", " ")
                .Replace("   ", " ")
                .Replace("  ", " ")
                .Replace("[b]", string.Empty)
                .Replace("[/b]", string.Empty)
                .Replace("[i]", string.Empty)
                .Replace("[i]", string.Empty)
                .Replace("[gt]", ">")
                .Replace("[lt]", "<")
                .Trim();

            formatted = _urlRegex.Replace(formatted, "$2");

            return formatted;
        }

        public AppPage Page { get; set; }

        public string Title { get; set; }

        public string FormattedDescription { get; set; }
        
        public string Description { get; set; }

        public string HtmlDescription { get; set; }

        public string ToolTipDescription { get; set; }

        public List<Features> Features { get; set; }

        public string TopLevelCategory { get; set; }

        public string GithubExampleUrl { get; set; }

        public string Group { get; set; }

        public string ExampleImagePath { get; set; }
        
        public string IconPath { get; set; }

        public ICommand SelectCommand { get; set; }

        public Guid PageId { get { return Page.PageId; } }

        public ExampleUsage Usage { get; set; }

        public Dictionary<string, string> SourceFiles
        {
            get
            {
                if(!_isLoaded)
                {
                    LoadCode();
                    _isLoaded = true;
                }

                return _sourceFiles;
            }
        }

        private void LoadCode()
        {
            try
            {
                _sourceFilePaths.ForEach(file =>
                {
                    var index = file.LastIndexOf('/') + 1;
                    var fileName = file.Substring(index).Replace(".txt", String.Empty);

                    _sourceFiles[fileName] = ExampleLoader.LoadSourceFile(file);
                });
            }
            catch (Exception ex)
            {                
                throw new Exception(string.Format("An error occurred when parsing the source-code files for Example '{0}'", Title), ex);
            }
        }

        //TODO Remove later, added for testing purpose
        public override string ToString()
        {
            return Title;
        }
    }
}