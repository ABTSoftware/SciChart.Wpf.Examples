using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SciChart.Charting.Common.Helpers;
using SciChart.Data.Model;
using SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer
{
    /// <summary>
    /// Additional enum passed to ChannelViewModel: Resampling precisions
    /// 
    /// see https://www.scichart.com/documentation/win/current/webframe.html#The%20Extreme%20Resamplers%20API.html for info
    /// on what the numeric values mean. Default is 0 
    /// </summary>
    public enum ResamplingPrecision
    {
        Default = 0,
        Precision2x = 1,
        Precision4x = 2,
    }

    public class DigitalAnalyzerExampleViewModel : BaseViewModel
    {
        private bool _isLoading;
        private DoubleRange _xRange;

        public DigitalAnalyzerExampleViewModel()
        {
            ChannelViewModels = new ObservableCollection<ChannelViewModel>();

            SelectedChannelType = "Digital";
            SelectedChannelCount = 32;
            SelectedPointCount = 1000000;
            SelectedResamplingPrecision =ResamplingPrecision.Default;
            SelectedStrokeThickness = 1;

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

                var isDigital = SelectedChannelType == "Digital";
                await AddChannels(isDigital ? 1 : 0, isDigital ? 0 : 1);

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

                // Create a bunch of Digital channels
                await AddChannels(SelectedChannelCount, 0);

                XRange = new DoubleRange(0, SelectedPointCount);
                IsLoading = false;
            });

            LoadChannelsCommand.Execute(null);
        }

        private async Task AddChannels(int digitalChannelsCount, int analogChannelsCount)
        {
            List<byte[]> digitalChannels = new List<byte[]>();
            List<float[]> analogChannels = new List<float[]>();

            // Make sure we allow UI to show progress bar
            await Task.Delay(200);

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

                    for (var i = 0; i < analogChannelsCount; i++)
                    {
                        var newArray = new float[SelectedPointCount];
                        analogChannels.Add(newArray);
                    }
                });

                // Generate random data and fill channels
                await GenerateData(digitalChannels, analogChannels);
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

        private async Task GenerateData(List<byte[]> digitalChannels, List<float[]> analogChannels)
        {
            var digitalChannelsCount = digitalChannels.Count;
            var analogChannelsCount = analogChannels.Count;

            var totalChannelCount = digitalChannelsCount + analogChannelsCount;
            var channelList = new List<ChannelViewModel>(totalChannelCount);
            var channelIndex = ChannelViewModels.Count;

            await Task.Run(async () =>
            {
                var xStart = 0d;
                var xStep = 1d;

                var digital = new List<Task<ChannelViewModel>>(digitalChannelsCount);
                var analog = new List<Task<ChannelViewModel>>(analogChannelsCount);

                foreach (var channel in digitalChannels)
                {
                    var id = channelIndex++;
                    digital.Add(Task.Run(() => ChannelGenerationHelper.Instance.GenerateDigitalChannel(xStart, xStep, channel, id)));
                }
                foreach (var channel in analogChannels)
                {
                    var id = channelIndex++;
                    analog.Add(Task.Run(() => ChannelGenerationHelper.Instance.GenerateAnalogChannel(xStart, xStep, channel, id)));
                }

                await Task.WhenAll(digital.Union(analog));

                foreach (var p in digital.Union(analog))
                {
                    channelList.Add(p.Result);
                }
            });

            channelList.ForEach(ch =>
            {
                ch.StrokeThickness = this.SelectedStrokeThickness;
                ch.ResamplingPrecision = (uint)this.SelectedResamplingPrecision;
                ChannelViewModels.Add(ch);
            });
        }

        public ObservableCollection<ChannelViewModel> ChannelViewModels { get; private set; }

        public string SelectedChannelType { get; set; }

        public int SelectedPointCount { get; set; }
        /// <summary>
        /// Additional property passed to ChannelViewModel: strokeThickness of the line (Default 1)
        /// </summary>
        public int SelectedStrokeThickness { get; set; }
        /// <summary>
        /// Additional property passed to ChannelViewModel: Resampling precisions
        /// 
        /// see https://www.scichart.com/documentation/win/current/webframe.html#The%20Extreme%20Resamplers%20API.html for info
        /// on what the numeric values mean. Default is 0 
        /// </summary>
        public ResamplingPrecision SelectedResamplingPrecision { get; set; }

        public int SelectedChannelCount { get; set; }

        public ActionCommand<object> ChangeChannelHeightCommand { get; }

        public ActionCommand AddChannelCommand { get; }

        public ActionCommand LoadChannelsCommand { get; }

        public long TotalPoints => ChannelViewModels.Sum(c => (long)c.DataCount);

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