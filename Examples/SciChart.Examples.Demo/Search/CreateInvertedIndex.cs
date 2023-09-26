using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SciChart.Core.Extensions;
using SciChart.Examples.Demo.Helpers;

namespace SciChart.Examples.Demo.Search
{
    public static class CreateInvertedIndex
    {
        //const string InvertedIndexRelativePath = @"\Resources\invertedIndex.dat";

        private static readonly string[] _stopWords;
        private static readonly string[] _codeStopWords;

        private static readonly Dictionary<string, Posting> _invertedIndex;
        private static readonly Dictionary<string, Posting> _codeInvertedIndex;

        static CreateInvertedIndex()
        {
            _stopWords = GetStopWords("stopwords", '\n');
            _codeStopWords = GetStopWords("codeStopwords", '\r');

            _invertedIndex = new Dictionary<string, Posting>();
            _codeInvertedIndex = new Dictionary<string, Posting>();
        }

        private static string[] GetStopWords(string fileName, char splitter)
        {
            Assembly assembly = typeof(CreateInvertedIndex).Assembly;

            var names = assembly.GetManifestResourceNames();

            var allExampleSourceFiles = names.Where(x => x.Contains(fileName));

            var file = allExampleSourceFiles.FirstOrDefault();

            var result = new string[] { };

            if (file != null)
            {
                using (var s = assembly.GetManifestResourceStream(file))
                using (var sr = new StreamReader(s))
                {
                    var readToEnd = sr.ReadToEnd();
                    result = readToEnd.Split(splitter);
                    result = result.Select(x => x.Replace("\n", "")).ToArray();
                }
            }

            return result;
        }

        public static IList<string> GetTerms(string text)
        {
            text = text.ToLower();
            text = new Regex(@"\W").Replace(text, " ");

            var words = text.Split(' ').Where(x => x != "").Where(word => !_stopWords.Contains(word)).ToArray();

            var tokenizer = new NGramTokenizer();
            var terms = words
                .Select(tokenizer.Tokenize)
                .SelectMany(strings => strings.SelectMany(inner => inner))
                .Select(sb => sb.ToString())
                .Where(s => !string.IsNullOrEmpty(s) && s.Length > 1)
                .ToList();

            return terms;
        }

        public static void CreateIndex(IEnumerable<KeyValuePair<Guid, Example>> examples)
        {
            var ex = examples.ToList();
            foreach (var example in ex)
            {
                string lines = GetTextFromExample(example.Value);
                var terms = GetTerms(lines);

                // Memory optimisation. Store term indices as ushort (16bit)
                if (terms.Count > ushort.MaxValue)
                    throw new InvalidOperationException("Too many terms in this example: " + example.Value.Title);

                var termDictExample = new Dictionary<string, List<ushort>>();
                for (ushort i = 0; i < terms.Count; i++)
                {
                    var term = terms[i];
                    if (termDictExample.ContainsKey(term))
                    {
                        termDictExample[term].Add(i);
                    }
                    else
                    {
                        termDictExample[term] = new List<ushort> { i };
                    }
                }

                var norm = Math.Sqrt(termDictExample.Sum(termDict => Sqr(termDict.Value.Count)));

                foreach (var termDict in termDictExample)
                {
                    var term = termDict.Key;
                    termDict.Value.TrimExcess();

                    if (_invertedIndex.ContainsKey(term))
                    {
                        var ti = new TermInfo(example.Key, termDict.Value.ToArray(), (float)(termDict.Value.Count / norm));
                        _invertedIndex[term].TermInfos.Add(ti);
                    }
                    else
                    {
                        _invertedIndex[term] = new Posting(new List<TermInfo>
                        {
                            new TermInfo(example.Key, termDict.Value.ToArray(), (float)(termDict.Value.Count / norm))
                        });
                    }
                    _invertedIndex[term].InvertedDocumentFrequency += 1;
                }
            }

            _invertedIndex.ForEachDo(x => x.Value.InvertedDocumentFrequency = Math.Log(ex.Count / x.Value.InvertedDocumentFrequency));
        }

        public static void CreateIndexForCode(IEnumerable<KeyValuePair<Guid, Example>> examples)
        {
            var ex = examples.ToList();

            foreach (var example in ex)
            {
                var tokenizer = new NGramTokenizer();

                string lines = GetSourceCodeFromExample(example.Value);
                var terms = lines.ToLower().Split(' ').Where(x => x != "")
                    .Select(tokenizer.Tokenize)
                    .SelectMany(strings => strings.SelectMany(inner => inner))
                    .Select(sb => sb.ToString())
                    .Where(s => !string.IsNullOrEmpty(s) && s.Length > 1)
                    .ToList();

                // Memory optimisation. Store term indices as ushort (16bit)
                if (terms.Count > ushort.MaxValue)
                    throw new InvalidOperationException("Too many code terms for example: " + example.Value.Title);

                var termDictExample = new Dictionary<string, List<ushort>>();
                for (ushort i = 0; i < terms.Count; i++)
                {
                    var term = terms[i];
                    if (termDictExample.ContainsKey(term))
                    {
                        termDictExample[term].Add(i);
                    }
                    else
                    {
                        termDictExample[term] = new List<ushort> { i };
                    }
                }

                var norm = Math.Sqrt(termDictExample.Sum(termDict => Sqr(termDict.Value.Count)));

                foreach (var termDict in termDictExample)
                {
                    var term = termDict.Key;
                    var list = termDict.Value;

                    if (_codeInvertedIndex.ContainsKey(term))
                    {
                        var ti = new TermInfo(example.Key, termDict.Value.ToArray(), (float)(termDict.Value.Count / norm));
                        _codeInvertedIndex[term].TermInfos.Add(ti);
                    }
                    else
                    {
                        var ti = new TermInfo(example.Key, termDict.Value.ToArray(), (float)(termDict.Value.Count / norm));
                        _codeInvertedIndex[term] = new Posting(new List<TermInfo>
                        {
                            ti,
                        });
                    }
                    _codeInvertedIndex[term].InvertedDocumentFrequency += 1;
                }

            }

            _codeInvertedIndex.ForEachDo(x =>
            {
                x.Value.InvertedDocumentFrequency = Math.Log(ex.Count / x.Value.InvertedDocumentFrequency);

                // Collapse memory of List<TermInfo>
                x.Value.TermInfos = x.Value.TermInfos.ToList();
            });
        }

        /*
        private static void WriteIndexToFile()
        {
            var location = Assembly.GetExecutingAssembly().Location;

            var index = location.IndexOf(@"\bin", StringComparison.InvariantCulture);

            var filePath = location.Substring(0, index) + InvertedIndexRelativePath;

            using (var outFile = new StreamWriter(filePath))
            {
                foreach (KeyValuePair<string, Posting> posting in _invertedIndex)
                {
                    var postingList = new List<string>();
                    foreach (var termInfo in posting.Value.TermInfos)
                    {
                        string indexes = string.Join(",", termInfo.TermEntryIndexes);
                        postingList.Add(string.Format("{0}:{1}", termInfo.ExamplePageId, indexes));
                    }

                    string postingData = string.Join(";", postingList);

                    var termFrequencies = posting.Value.TermInfos.Select(x => x.TermFrequency);
                    string termFrequency = string.Join(",", termFrequencies);

                    outFile.WriteLine("{0}|{1}|{2}|{3}", posting.Key, postingData, termFrequency,
                        posting.Value.InvertedDocumentFrequency);
                }
            }
        }

        public static void ReadIndexFromFile()
        {
            var location = Assembly.GetExecutingAssembly().Location;

            var index = location.IndexOf(@"\bin", StringComparison.InvariantCulture);

            var filePath = location.Substring(0, index) + InvertedIndexRelativePath;

            string[] lines = File.ReadAllLines(filePath);

            _invertedIndex.Clear();
            foreach (var line in lines)
            {
                var splittedLine = line.Split('|');

                string term = splittedLine[0];
                var postings = splittedLine[1].Split(';');
                var termFrequencies = splittedLine[2].Split(',');
                var invertedDocFrequency = double.Parse(splittedLine[3]);

                var termInfos = new List<TermInfo>();

                for (int i = 0; i < postings.Length; i++)
                {
                    var posting = postings[i];
                    var tf = double.Parse(termFrequencies[i]);

                    var post = posting.Split(':');
                    var termEntries = post[1].Split(',').Select(ushort.Parse).ToArray();

                    termInfos.Add(new TermInfo(new Guid(post[0]), termEntries, (float)tf));
                }

                _invertedIndex[term] = new Posting(termInfos) { InvertedDocumentFrequency = invertedDocFrequency };
            }
        }
        */

        private static string GetTextFromExample(Example example)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0} ", example.Title);
            sb.AppendFormat("{0} ", example.Group);
            example.Features.ForEach(feature => sb.AppendFormat("{0} ", feature));
            sb.AppendFormat("{0} ", example.Description);

            return sb.ToString();
        }

        private static string GetSourceCodeFromExample(Example example)
        {
            var description = example.SourceFiles;
            var uiCodeFiles = description.Where(x => x.Key.EndsWith(".xaml")).ToList();

            var sb = new StringBuilder();

            foreach (var uiFile in uiCodeFiles)
            {
                var xml = XDocument.Parse(uiFile.Value);

                foreach (var node in xml.Root.Nodes())
                {
                    if (node is XComment)
                    {
                        continue;
                    }

                    var xElements = ((XElement)node).Descendants();
                    foreach (var element in xElements)
                    {
                        if (!_codeStopWords.Contains(element.Name.LocalName))
                        {
                            sb.AppendFormat("{0} ", element.Name.LocalName);
                            foreach (var attribute in element.Attributes())
                            {
                                sb.AppendFormat("{0} ", attribute.Name.LocalName);
                            }
                        }
                    }
                }
            }

            var lines = sb.ToString();
            return lines;
        }

        public static Dictionary<string, Posting> GetInvertedIndex()
        {
            return _invertedIndex;
        }

        public static Dictionary<string, Posting> GetCodeInvertedIndex()
        {
            return _codeInvertedIndex;
        }

        private static int Sqr(int value)
        {
            return value * value;
        }
    }
}