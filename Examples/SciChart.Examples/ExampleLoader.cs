// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ExampleLoader.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace SciChart.Examples
{
    public class ExampleKey
    {
        protected bool Equals(ExampleKey other)
        {
            return string.Equals(ExampleCategory, other.ExampleCategory) && string.Equals(ChartGroup, other.ChartGroup) && string.Equals(ExampleTitle, other.ExampleTitle);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ExampleCategory != null ? ExampleCategory.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ChartGroup != null ? ChartGroup.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ExampleTitle != null ? ExampleTitle.GetHashCode() : 0);
                return hashCode;
            }
        }
        public string ExampleCategory { get; set; }
        public string ChartGroup { get; set; }
        public string ExampleTitle { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExampleKey) obj);
       } 
    }

    public class ExampleLoader
    {
        public ExampleDefinition Parse(KeyValuePair<ExampleKey, string> xmlExample)
        {
            return Deserialize(xmlExample);
        }

        private ExampleDefinition Deserialize(KeyValuePair<ExampleKey, string> xmlExample)
        {
            try
            {
                // Load XmlDeserializer - workaround that prevents an Exception in System.Xml
                XmlSerializer deserializer = XmlSerializer.FromTypes(new[] { typeof(ExampleDefinition) })[0];

                var stringReader = new StringReader(xmlExample.Value);
                var xmlReader = XmlReader.Create(stringReader);

                var res = deserializer.Deserialize(xmlReader) as ExampleDefinition;

                xmlReader.Close();
                stringReader.Close();

                res.ExampleCategory = xmlExample.Key.ExampleCategory;
                res.ChartGroup = xmlExample.Key.ChartGroup;
                res.Title = xmlExample.Key.ExampleTitle;

                string codeFile = res.CodeFiles.FirstOrDefault();
                if (codeFile == null)
                {
                    res.GithubUrl = Urls.GithubRootUrl;
                }
                else
                {
                    string exampleFolder = codeFile.Replace("Resources/ExampleSourceFiles/", "");
                    exampleFolder = exampleFolder.Substring(0, exampleFolder.LastIndexOf("/", StringComparison.InvariantCulture));
                    res.GithubUrl = Urls.GithubExampleRootUrl + exampleFolder;
                }

                return res;
            }
            catch (Exception caught)
            {
                throw new Exception("An error occurred when deserializing Example XML " + xmlExample.Value, caught);
            }
        }

        public static string LoadSourceFile(string name)
        {
            Assembly assembly = typeof(ExampleLoader).Assembly;

            var names = assembly.GetManifestResourceNames();

            var allExampleSourceFiles = names.Where(x => x.Contains("ExampleSourceFiles"));

            var find = name.Replace('/', '.').Replace(".cs.", ".c.");
            var file = allExampleSourceFiles.FirstOrDefault(x => x.Contains(find));

            if (file == null)
                throw new Exception(string.Format("Unable to find the source code resource {0}", find));

            using (var s = assembly.GetManifestResourceStream(file))
            using (var sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }

        public IDictionary<ExampleKey, string> DiscoverAllXmlFiles()
        {
            return DiscoverAllExampleDefinitions();
        }

        private IDictionary<ExampleKey, string> DiscoverAllExampleDefinitions()
        {
            var dict = new Dictionary<ExampleKey, string>();
            Assembly assembly = typeof (ExampleLoader).Assembly;

            var names = assembly.GetManifestResourceNames();

            var allXmlManifestResources = names.Where(x => x.Contains("ExampleDefinitions")).ToList();
            allXmlManifestResources.Sort();

            foreach(var xmlResource in allXmlManifestResources)
            {
                using (var s = assembly.GetManifestResourceStream(xmlResource))
                using (var sr = new StreamReader(s))
                {
                    string exampleKeyString = xmlResource.Replace("SciChart.Examples.Resources.ExampleDefinitions.", string.Empty)
                        .Replace("SciChart.Examples.SL.Resources.ExampleDefinitions.", string.Empty);

                    string[] chunks = exampleKeyString.Split('.');
                    var exampleKey = new ExampleKey()
                    {
                        ExampleCategory = Trim(chunks[0], true),
                        ChartGroup = Trim(chunks[1]),
                        ExampleTitle = Trim(chunks[2])
                    };
                    dict.Add(exampleKey, sr.ReadToEnd());
                }
            }
            return dict;
        }

        private string Trim(string str, bool skipFirstChar = false)
        {
            var trimmed = str.Replace("_", " ").Trim();
            if (skipFirstChar)
            {
                trimmed = trimmed.Substring(1, trimmed.Length - 1);
            }
            return trimmed;
        }
    }
}
