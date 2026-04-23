// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// Posting.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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

        public List<TermInfo> TermInfos { get; set; }

        /// <summary>
        /// The document frequency of a term t is the log from the number of documents divided by 
        /// the number of documents containing the term
        /// </summary>
        public double InvertedDocumentFrequency { get; set; }
    }
}