using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SciChart.Core.Extensions;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Examples.Demo.Search
{

    public struct ExampleId
    {
        public Guid Id;
    }

    public interface IExampleSearchProvider
    {
        IEnumerable<ExampleId> Query(string text);
        IEnumerable<ExampleId> OneWordQuery(string[] terms);
        IEnumerable<ExampleId> FreeTextQuery(string[] terms);
    }

    [ExportType(typeof(IExampleSearchProvider), CreateAs.Singleton)]
    public class ExampleSearchProvider : IExampleSearchProvider
    {
        private readonly Dictionary<string, Posting> _invertedIndex;
        private readonly Dictionary<string, Posting> _codeInvertedIndex;

        public ExampleSearchProvider()
        {
            _invertedIndex = CreateInvertedIndex.GetInvertedIndex();
            _codeInvertedIndex = CreateInvertedIndex.GetCodeInvertedIndex();
        }

        public IEnumerable<ExampleId> Query(string text)
        {
            text = text.ToLower();
            text = new Regex(@"\W").Replace(text, " ");

            var terms = text.Split(' ').ToArray();

            IEnumerable<ExampleId> result = null;
            if (terms.Length > 1)
            {
                result = FreeTextQuery(terms);
            }
            else if (terms.Length == 1)
            {
                result = OneWordQuery(terms);
            }

            return result;
        }

        public IEnumerable<ExampleId> OneWordQuery(string[] terms)
        {
            IEnumerable<ExampleId> result = null;

            var pageIds = new List<Guid>();
            var term = terms[0];

            if (_invertedIndex.ContainsKey(term))
            {
                _invertedIndex[term].TermInfos.ForEachDo(posting => pageIds.Add(posting.ExamplePageId));
                result = RankDocuments(terms, pageIds, _invertedIndex).Select(guid => new ExampleId { Id = guid });
            }

            var codePageIds = new List<Guid>();
            if (_codeInvertedIndex.ContainsKey(term))
            {
                _codeInvertedIndex[term].TermInfos.ForEachDo(posting =>
                {
                    if (!pageIds.Contains(posting.ExamplePageId))
                    {
                        codePageIds.Add(posting.ExamplePageId);
                    }
                });
                var codeResults = RankDocuments(terms, codePageIds, _codeInvertedIndex).Select(guid => new ExampleId { Id = guid });
                result = result != null ? result.Concat(codeResults) : codeResults;
            }
            return result;
        }

        public IEnumerable<ExampleId> FreeTextQuery(string[] terms)
        {
            IEnumerable<ExampleId> result = null;

            if (terms.Length != 0)
            {
                var pageIds = new HashSet<Guid>();

                foreach (var term in terms)
                {
                    if (_invertedIndex.ContainsKey(term))
                    {
                        _invertedIndex[term].TermInfos.ForEachDo(posting => pageIds.Add(posting.ExamplePageId));
                    }
                }

                result = RankDocuments(terms, pageIds, _invertedIndex).Select(guid => new ExampleId() { Id = guid });
            }

            return result;
        }

        private IEnumerable<Guid> RankDocuments(string[] terms, IEnumerable<Guid> examplePageIds, IDictionary<string, Posting> invertedIndex)
        {
            var queryVector = new double[terms.Length];
            var docVectors = new Dictionary<Guid, double[]>();
            var docScores = new Dictionary<Guid, double>();

            for (int i = 0; i < terms.Length; i++)
            {
                string term = terms[i];
                if (!invertedIndex.ContainsKey(term))
                {
                    continue;
                }

                var posting = invertedIndex[term];
                queryVector[i] = (posting.InvertedDocumentFrequency);

                foreach (var termInfo in posting.TermInfos)
                {
                    var examplePageId = termInfo.ExamplePageId;
                    if (examplePageIds.Contains(examplePageId))
                    {
                        if (!docVectors.ContainsKey(examplePageId))
                        {
                            docVectors[examplePageId] = new double[terms.Length];
                        }
                        docVectors[examplePageId][i] = termInfo.TermFrequency;
                    }
                }
            }

            foreach (var docVector in docVectors)
            {
                var dotProduct = DotProduct(docVector.Value, queryVector);
                docScores.Add(docVector.Key, dotProduct);
            }

            return docScores.OrderByDescending(pair => pair.Value).Select(pair => pair.Key);
        }

        private double DotProduct(double[] vector1, double[] vector2)
        {
            double result = vector1.Length != vector2.Length ? 0 : vector1.Zip(vector2, (x, y) => x * y).Sum();
            return result;
        }

    }
}