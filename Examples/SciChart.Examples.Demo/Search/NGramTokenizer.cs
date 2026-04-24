// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// NGramTokenizer.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Linq;
using System.Text;

namespace SciChart.Examples.Demo.Search
{
    public class NGramTokenizer
    {
        public StringBuilder[][] Tokenize(string word)
        {
            var m = word.Length;
            var n = m - 1;

            var array = new StringBuilder[m][];
            for (int i = 0; i < m; i++)
            {
                array[i] = new StringBuilder[m];
            }

            for (int j = 0; j < m; j++)
            {
                for (var k = 0; k < m; k++)
                {
                    array[j][k] = new StringBuilder();
                }
            }

            for (int j = 0; j < m; j++)
            {
                var letter = word[j];

                for (var k = 0; k <= j; k++)
                {
                    for (int i = j - k; i < n && k < m - i; i++)
                    {
                        array[i][k].Append( letter);
                    }
                }
            }
            array[m- 1][0] = new StringBuilder(word);

            return array;
        }
    }
}