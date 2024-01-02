// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AudioDeviceInfo.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using NAudio.CoreAudioApi;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    public class AudioDeviceInfo : BaseViewModel
    {
        private string _displayName;
        private string _id;

        public AudioDeviceInfo(MMDevice device)
        {
            _displayName = device.DeviceFriendlyName;
            _id = device.ID;
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        internal void Update(MMDevice device)
        {
            _displayName = device.DeviceFriendlyName;
        }
    }
}