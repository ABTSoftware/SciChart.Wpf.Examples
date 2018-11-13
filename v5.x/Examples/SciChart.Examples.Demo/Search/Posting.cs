using System.Collections.Generic;

namespace SciChart.Examples.Demo.Search
{
    public class Posting
    {
        public Posting() {}

        public Posting(List<TermInfo> termInfos)
        {
            TermInfos = termInfos;
        }

        public IList<TermInfo> TermInfos { get; set; }

        /// <summary>
        /// The document frequency of a term t is the log from the number of documents divided by 
        /// the number of documents containing the term
        /// </summary>
        public double InvertedDocumentFrequency { get; set; }
    }
}