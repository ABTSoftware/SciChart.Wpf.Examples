// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ExampleDefinition.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SciChart.Examples
{
    [Serializable]
    public class ExampleDefinition
    {
        public string GithubUrl { get; set; }
        public string ExampleCategory { get; set; }
        public string ChartGroup { get; set; }
        public string Title { get; set; }
        public string View { get; set; }
        public string ViewModel { get; set; }
        public string ImagePath { get; set; }
        public string IconPath { get; set; }
        public string Description { get; set; }
        public string ToolTipDescription { get; set; }
        public List<string> CodeFiles { get; set; }
        public List<Features> Features { get; set; }
        public SafeXmlBool IsShowcaseExample { get; set; }
        public string ShowcaseImagePath { get; set; }
        public string ShowcaseDescription { get; set; }
    }

    public struct SafeXmlBool : IXmlSerializable
    {
        private bool _value;

        public static implicit operator bool(SafeXmlBool safeBool)
        {
            return safeBool._value;
        }

        public static implicit operator SafeXmlBool(bool value)
        {
            return new SafeXmlBool { _value = value };
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            _value = reader.ReadElementContentAsString().ToLowerInvariant()  == "true";
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_value ? "True" : "False");
        }
    }
}