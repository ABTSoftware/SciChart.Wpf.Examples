using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace StringsOnXAxisExample
{
    public partial class StringsOnXAxis : Window
    {
        public StringsOnXAxis()
        {
            InitializeComponent();

            columnSeries.DataSeries = GetSomeData();

            // Setup the labels using LabelProvider. Values 0,1,2,3,4 will correspond to labels below
            var labels = new string[] { "Apples", "Oranges", "Pears", "Bananas", "Kiwis" }.ToList();
            scs.XAxis.LabelProvider = new StringLabelProvider(labels.ToDictionary(label => labels.IndexOf(label)));
        }

        private IDataSeries GetSomeData()
        {
            var data = new XyDataSeries<int, double>();
            
            for (int i = 0; i < 5; i++)
            {
                data.Append(i, i+1);
            }
            return data;
        }
    }

    // Use the LabelProvider API to format the labels using the provided labels map
    public class StringLabelProvider : NumericLabelProvider
    {
        private readonly Dictionary<int, string> _labelsMap;

        public StringLabelProvider(Dictionary<int, string> labelsMap)
        {
            _labelsMap = labelsMap;
        }

        public override string FormatLabel(IComparable dataValue)
        {
            int labelValue = (int)Convert.ChangeType(dataValue, typeof(int));
            return _labelsMap.ContainsKey(labelValue) ? _labelsMap[labelValue] : labelValue + " Not in map!";
        }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            int labelValue = (int)Convert.ChangeType(dataValue, typeof(int));
            return _labelsMap.ContainsKey(labelValue) ? _labelsMap[labelValue] : labelValue + " Not in map!";
        }
    }
}
