using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using CodeHighlighter.Common;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class TextFormattingConverterBase
    {
        public Color? HighlightColor { get; set; }

        public string HighlightTermsBase(string text, string[] terms)
        {
            var set = new HashSet<int>();
            foreach (var term in terms)
            {
                var indexes = text.ToLower().AllIndexesOf(term).Reverse().ToList();
                foreach (var index in indexes)
                {
                    for (int j = 0; j < term.Length; j++)
                    {
                        set.Add(j + index);
                    }
                }
            }

            if (set.Count > 0)
            {
                var list = set.ToList();
                list.Sort();

                var ranges = GetRanges(list.ToArray());
                ranges.Reverse();

                var color = HighlightColor ?? Colors.Black;
                string colorString = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);

                foreach (var range in ranges)
                {
                    text = text.Insert(range.Item1 + range.Item2, "</Run>");
                    text = text.Insert(range.Item1, string.Format("<Run Foreground=\"{0}\">", colorString));
                }
            }

            return text;
        }

        public List<Tuple<int, int>> GetRanges(int[] array)
        {
            if (array.Length < 1)
                return null;

            var indexsesList = new List<List<int>>();

            var someList = new List<int>();
            for (int i = 0; i < array.Length - 1; i++)
            {
                someList.Add(array[i]);

                if (array[i + 1] - array[i] > 1)
                {
                    indexsesList.Add(someList);
                    someList = new List<int>();
                }
            }
            someList.Add(array[array.Length - 1]);
            indexsesList.Add(someList);

            return indexsesList.Select(list => new Tuple<int, int>(list[0], list.Count)).ToList();
        }
    }
}