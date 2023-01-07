using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace SciChart.Examples.Demo.Helpers
{
    public class RichTextBoxHelper
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text",
            typeof (string), typeof (RichTextBoxHelper), new PropertyMetadata(null, OnTextChanged));

        public static string GetText(DependencyObject o)
        {
            return (string) o.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject o, string value)
        {
            o.SetValue(TextProperty, value);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rtb = d as RichTextBox;

            if (rtb != null)
            {
#if !SILVERLIGHT
                rtb.IsDocumentEnabled = true;
#endif
                var text = (string) e.NewValue;
                var xamlString = GetXamlFrom(text);

                LoadXamlInto(rtb, xamlString);


#if SILVERLIGHT
    //Scroll RichTextBox to top
                rtb.Selection.Select(rtb.ContentStart, rtb.ContentStart);
#endif
            }
        }

        private static void LoadXamlInto(RichTextBox rtb, string xamlStr)
        {
#if !SILVERLIGHT
            var msDocument = new MemoryStream((new ASCIIEncoding()).GetBytes(xamlStr));

            var formattedDocument = XamlReader.Load(msDocument) as

                FlowDocument;

            rtb.Document = formattedDocument;
            SubscribeToAllHyperlinks(formattedDocument);
            msDocument.Close();
#else
            var formattedDocument = XamlReader.Load(xamlStr) as
            Paragraph;

            rtb.Blocks.Clear();
            rtb.Blocks.Add(formattedDocument);
#endif
        }

        public static string GetXamlFrom(string text)
        {
            var xamlStr = new StringBuilder();

#if !SILVERLIGHT
            xamlStr.Append("<FlowDocument xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">");
#endif
            xamlStr.Append("<Paragraph xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">");
            xamlStr.Append(text);
            xamlStr.Append("</Paragraph>");
#if !SILVERLIGHT
            xamlStr.Append("</FlowDocument>");
#endif
            return xamlStr.ToString();
        }

#if !SILVERLIGHT
        private static void SubscribeToAllHyperlinks(FlowDocument doc)
        {
            var hyperlinks = GetVisuals(doc).OfType<Hyperlink>();
            foreach (var link in hyperlinks)
            {
                link.TargetName = null;
                link.RequestNavigate += LinkOnClick;
            }
        }

        private static void LinkOnClick(object sender, RequestNavigateEventArgs e)
        {
            Process.Start("explorer.exe", e.Uri.AbsoluteUri);
            e.Handled = true;
        }


        private static IEnumerable<DependencyObject> GetVisuals(DependencyObject root)
        {
            foreach (var child in LogicalTreeHelper.GetChildren(root).OfType<DependencyObject>())
            {
                yield return child;
                foreach (var descendants in GetVisuals(child))
                {
                    yield return descendants;
                }
            }
        }
#endif
    }
}