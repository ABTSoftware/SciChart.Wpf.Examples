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
                var blocks =
#if SILVERLIGHT
                    rtb.Blocks;
#else
                    rtb.Document.Blocks;
#endif
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
                cc.Colorize(content, language, xif, new VS2013DarkStyleSheet());

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