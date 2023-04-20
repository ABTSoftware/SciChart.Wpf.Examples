using SciChart.Charting.Common.Helpers;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows;

namespace SciChart_DigitalAnalyzerPerformanceDemo
{
    public class DigitalAnalyzerExampleViewModel : BaseViewModel
    {
        private bool _isLoading;
        private DoubleRange _xRange;

        public DigitalAnalyzerExampleViewModel()
        {
            ChannelViewModels = new ObservableCollection<ChannelViewModel>();

            SelectedChannelCount = 32;
            SelectedPointCount = 1000000;

            _ = AddChannels(SelectedChannelCount);
            XRange = new DoubleRange(0, SelectedPointCount);

            ChangeChannelHeightCommand = new ActionCommand<object>((d) =>
            {
                var delta = (double)d;
                foreach (var channelViewModel in ChannelViewModels)
                {
                    channelViewModel.SetChannelHeightDelta(delta);
                }
            });

            AddChannelCommand = new ActionCommand(async () =>
            {
                IsLoading = true;

                await AddChannels(1);

                IsLoading = false;
            });

            LoadChannelsCommand = new ActionCommand(async () =>
            {
                IsLoading = true;

                // Clear ViewModels
                foreach (var channelVm in ChannelViewModels)
                {
                    channelVm.Clear();
                }
                ChannelViewModels.Clear();
                XRange = null;

                // Create new channels
                await AddChannels(SelectedChannelCount);

                XRange = new DoubleRange(0, SelectedPointCount);
                IsLoading = false;
            });
        }

        private async Task AddChannels(int digitalChannelsCount)
        {
            var digitalChannels = new List<byte[]>();

            // Force GC to free memory
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

            try
            {
                // Allocate memory first
                await Task.Run(() =>
                {
                    for (var i = 0; i < digitalChannelsCount; i++)
                    {
                        var newArray = new byte[SelectedPointCount];
                        digitalChannels.Add(newArray);
                    }
                });

                // Generate random data and fill channels
                await GenerateData(digitalChannels);
            }
            catch (OutOfMemoryException)
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() => ChannelViewModels.Clear()));
                MessageBox.Show(string.Format($"There is not enough RAM memory to allocate {SelectedChannelCount} channels with {SelectedPointCount} points each. Please select less channels or less points and try again."));
            }
            finally
            {
                OnPropertyChanged(nameof(IsEmpty));
                OnPropertyChanged(nameof(ChannelViewModels));
                OnPropertyChanged(nameof(SelectedPointCount));
                OnPropertyChanged(nameof(TotalPoints));
                OnPropertyChanged(nameof(XRange));

                IsLoading = false;
            }
        }

        private async Task GenerateData(List<byte[]> digitalChannels)
        {
            var digitalChannelsCount = digitalChannels.Count;

            var channelList = new List<ChannelViewModel>(digitalChannelsCount);
            var channelIndex = ChannelViewModels.Count;

            await Task.Run(async () =>
            {
                var xStart = 0d;
                var xStep = 1d;

                var channels = new List<Task<ChannelViewModel>>(digitalChannelsCount);

                foreach (var channel in digitalChannels)
                {
                    var id = channelIndex++;
                    channels.Add(Task.Run(() => ChannelGenerationHelper.Instance.GenerateDigitalChannel(xStart, xStep, channel, id)));
                }

                await Task.WhenAll(channels);

                foreach (var p in channels)
                {
                    channelList.Add(p.Result);
                }
            });

            channelList.ForEach(ch => ChannelViewModels.Add(ch));
        }

        public ObservableCollection<ChannelViewModel> ChannelViewModels { get; private set; }

        public int SelectedPointCount { get; set; }

        public int SelectedChannelCount { get; set; }

        public ActionCommand<object> ChangeChannelHeightCommand { get; }

        public ActionCommand AddChannelCommand { get; }

        public ActionCommand LoadChannelsCommand { get; }

        public long TotalPoints => ChannelViewModels.Count * (long)SelectedPointCount;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public bool IsEmpty => ChannelViewModels.Count <= 0;

        public DoubleRange XRange
        {
            get => _xRange;
            set
            {
                _xRange = value;
                OnPropertyChanged(nameof(XRange));
            }
        }
    }
}