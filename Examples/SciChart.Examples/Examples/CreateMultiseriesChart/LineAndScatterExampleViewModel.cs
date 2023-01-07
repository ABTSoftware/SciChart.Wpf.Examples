// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LineAndScatterExampleViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    public class LineAndScatterExampleViewModel : BaseViewModel
    {
        private readonly double[] _scatterY =
        {
            53, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, 70,
            double.NaN, 72, 81, 60, 67, double.NaN, double.NaN, 72, double.NaN, 72, double.NaN, double.NaN, double.NaN,
            92, double.NaN, 76, double.NaN, double.NaN, double.NaN, 85, double.NaN, 82, 90, 96, double.NaN, double.NaN,
            82, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, 130, double.NaN, double.NaN, double.NaN, 194, 198, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN, 208, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, 231, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, 231
        };

        private readonly double[] _lineY =
        {
            77.033282491478, 73.9085208618748, 71.1324976363107, 68.6967626265043,
            66.5928656441749, 64.8123565010411, 63.3467850088222, 62.187700979237, 61.3266542240045, 60.7551945548437,
            60.4648717834736, 60.447235721613, 60.6938361809811, 61.1962229732969, 61.9459459102786, 62.9345548036464,
            64.1535994651183, 65.5946297064143, 67.2491953392517, 69.1088461753512, 71.1651320264308, 73.4096027042101,
            75.833808020407, 78.4292977867417, 81.1876218149323, 84.1003299166986, 87.1589719037584, 90.3550975878318,
            93.6802567806369, 97.1259992938938, 100.68387493932, 104.345433528635, 108.102224873559, 111.94579878581,
            115.867705077105, 119.859493559167, 123.912714043712, 128.01891634246, 132.16965026713, 136.35646562944,
            140.57091224111, 144.80453991386, 149.048898459406, 153.295537689469, 157.536007415769, 161.761857450022,
            165.964637603949, 170.135897689268, 174.2671875177, 178.350056900961, 182.376055650773, 186.336733578852,
            190.223640496919, 194.028326216692, 197.74234054989, 201.357233308233, 204.864554303439, 208.255853347227,
            211.522680251317, 214.656584827426, 217.649116887275, 220.491826242582, 223.176262705066, 225.693976086446,
            228.036516198441, 230.19543285277, 232.162275861152, 233.928595035306, 235.485940186951, 236.825861127806,
            237.939907669589, 238.81962962402, 239.456576802818, 239.842299017702, 239.968346080391, 239.826267802603
        };

        private IUniformXyDataSeries<double> _scatterData;
        private IUniformXyDataSeries<double> _fittedData;

        public LineAndScatterExampleViewModel()
        {
            GenerateData();
        }

        public IUniformXyDataSeries<double> ScatterData
        {
            get => _scatterData;
            set
            {
                if (_scatterData == value) return;
                _scatterData = value;
                OnPropertyChanged("ScatterData");
            }
        }

        public IUniformXyDataSeries<double> FittedData
        {
            get => _fittedData;
            set
            {
                if (_fittedData == value) return;
                _fittedData = value;
                OnPropertyChanged("FittedData");
            }
        }

        private void GenerateData()
        {
            _scatterData = new UniformXyDataSeries<double>(57d, 1d);
            _fittedData = new UniformXyDataSeries<double>(57d, 1d);

            _scatterData.Append(_scatterY);
            _fittedData.Append(_lineY);
        }
    }
}