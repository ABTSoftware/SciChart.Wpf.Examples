// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AudioDeviceSource.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    class AudioDeviceSource : IDisposable, IMMNotificationClient
    {
        private MMDeviceEnumerator _enumerator = new MMDeviceEnumerator();
        private Dispatcher _dispatcher;

        public ObservableCollection<AudioDeviceInfo> Devices { get; } = new ObservableCollection<AudioDeviceInfo>();
        public event EventHandler DevicesChanged;

        public string DefaultDevice { get; private set; }
        
        public AudioDeviceSource()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _enumerator.RegisterEndpointNotificationCallback(this);
            RefreshDevices();
        }

        public AudioDeviceHandler CreateHandlder(string id)
        {
            var device = _enumerator.GetDevice(id);
            return new AudioDeviceHandler(device);
        }

        private void RefreshDevices()
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke((Action)RefreshDevices);
                return;
            }
            DefaultDevice = GetDefaultDevice();

            var deviceMap = Devices.ToDictionary(d => d.ID, d => d);
            var presentDevices = new HashSet<string>();

            foreach (var d in _enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                presentDevices.Add(d.ID);
                if(deviceMap.TryGetValue(d.ID, out var device))
                {
                    device.Update(d);
                }
                else
                {
                    Devices.Add(new AudioDeviceInfo(d));
                }
                d.Dispose();
            }

            for (int i = Devices.Count - 1; i >=0; i--)
            {
                if (!presentDevices.Contains(Devices[i].ID))
                {
                    Devices.RemoveAt(i);
                }
            }

            DevicesChanged?.Invoke(this, EventArgs.Empty);
        }

        private string GetDefaultDevice()
        {
            if(!_enumerator.HasDefaultAudioEndpoint(DataFlow.Capture, Role.Communications))
            {
                return null;
            }
            using (var device = _enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications))
            {
                return device.ID;
            }
        }

        public void Dispose()
        {
            _enumerator.UnregisterEndpointNotificationCallback(this);
            _enumerator.Dispose();
        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            RefreshDevices();
        }

        public void OnDeviceAdded(string pwstrDeviceId)
        {
            RefreshDevices();
        }

        public void OnDeviceRemoved(string deviceId)
        {
            RefreshDevices();
        }

        public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
            RefreshDevices();
        }

        public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
        {
            RefreshDevices();
        }
    }
}
