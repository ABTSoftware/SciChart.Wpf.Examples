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