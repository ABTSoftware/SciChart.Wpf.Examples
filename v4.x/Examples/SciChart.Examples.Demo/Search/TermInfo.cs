using System;
using System.Collections.Generic;

namespace SciChart.Examples.Demo.Search
{
    public class TermInfo
    {
        public TermInfo() {}

        // By accepting ushort[] not List<ushort> this collapses memory for terms 
        public TermInfo(Guid examplePageId, ushort[] termEntryIndexes, float termFrequency)
        {
            ExamplePageId = examplePageId;
            TermEntryIndexes = termEntryIndexes;
            TermFrequency = termFrequency;
        }

        public Guid ExamplePageId { get; private set; }

        /// <summary>
        /// Term entry indexes describe the positions of term in the document
        /// </summary>
        public IList<ushort> TermEntryIndexes { get; private set; }

        /// <summary>
        /// Term frequency in the document D is number of term in doc divided by Euclidian norm of document vector
        /// which is calculated by taking the square of each value in the document vector, summing them up, 
        /// and taking the square root of the sum.
        /// </summary>
        /// <remarks>Changed to float to reduce memory usage of TermInfo</remarks>
        public float TermFrequency { get; private set; }
    }
}