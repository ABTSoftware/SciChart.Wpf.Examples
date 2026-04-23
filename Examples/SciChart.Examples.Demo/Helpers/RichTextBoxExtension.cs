// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// RichTextBoxExtension.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows.Controls;
using System.Windows.Documents;
using CodeHighlighter;
using CodeHighlighter.Formatting;
using CodeHighlighter.Styling;

namespace SciChart.Examples.Demo.Helpers
{
    public static class RichTextBoxExtension
    {
        private static WeakReference _colorizer;

        public static Paragraph Highlight(this RichTextBox rtb)
        {
            Paragraph paragraph = null;

            if (rtb != null)
            {
                var blocks = rtb.Document.Blocks;
                blocks.Clear();

                paragraph = new Paragraph();
                var xif = new XamlInlineFormatter(paragraph);

                CodeColorizer cc;
                if (_colorizer != null && _colorizer.IsAlive)
                {
                    cc = (CodeColorizer) _colorizer.Target;
                }
                else
                {
                    cc = new CodeColorizer();
                    _colorizer = new WeakReference(cc);
                }

                var languageType = (SourceLanguageType) rtb.GetValue(CodeHighlighter.SourceLanguageProperty);
                ILanguage language = CreateLanguageInstance(languageType);

                var content = (string) rtb.GetValue(CodeHighlighter.SourceCodeProperty);
                cc.Colorize(content, language, xif, StyleSheets.GetStyleSheet());

                blocks.Add(paragraph);
            }

            return paragraph;
        }

        private static ILanguage CreateLanguageInstance(SourceLanguageType type)
        {
            switch (type)
            {
                case SourceLanguageType.CSharp:
                    return Languages.CSharp;

                case SourceLanguageType.Cpp:
                    return Languages.Cpp;

                case SourceLanguageType.JavaScript:
                    return Languages.JavaScript;

                case SourceLanguageType.VisualBasic:
                    return Languages.VbDotNet;

                case SourceLanguageType.Xaml:
                case SourceLanguageType.Xml:
                    return Languages.Xml;

                default:
                    throw new InvalidOperationException("Could not locate the provider.");
            }
        }
    }
}