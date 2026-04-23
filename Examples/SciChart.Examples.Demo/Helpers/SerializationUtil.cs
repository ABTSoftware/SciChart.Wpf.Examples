// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SerializationUtil.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using SciChart.Examples.Demo.Common;
using SciChart.UI.Reactive.Services;

namespace SciChart.Examples.Demo.Helpers
{
    public class Item
    {
        [XmlAttribute]
        public string ExampleName { get; set; }

        [XmlElement]
        public ExampleUsage ExampleUsage { get; set; }
    }

    public static class SerializationUtil
    {
        public static XDocument Serialize(IEnumerable<ExampleUsage> usages)
        {
            var doc = new XDocument();

            using (var writer = doc.CreateWriter())
            {
                var serializer = new XmlSerializer(typeof(Item[]), new XmlRootAttribute { ElementName = "items" });

                serializer.Serialize(writer, usages.Select(e => new Item { ExampleName = e.ExampleID, ExampleUsage = e }).ToArray());
            }

            return doc;
        }

        public static Dictionary<string, ExampleUsage> Deserialize(XDocument doc)
        {
            using (var reader = doc.Root.CreateReader())
            {
                var serializer = new XmlSerializer(typeof(Item[]), new XmlRootAttribute { ElementName = "items" });

                return ((Item[])serializer.Deserialize(reader)).ToDictionary(x => x.ExampleName, x => x.ExampleUsage);
            }
        }
    }
}