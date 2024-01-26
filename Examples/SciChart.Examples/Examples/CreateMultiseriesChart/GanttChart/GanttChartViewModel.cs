using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Core.Extensions;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    public class GanttChartViewModel : BaseViewModel
    {
        private DateRange _xVisibleRange;
        private DateTime _xCurrentDate;
        private DateTime _xEndDate;

        public IList<GanttItemViewModel> Items { get; }
        public IList<IAxisViewModel> YAxes { get; private set; }
        public IList<IRenderableSeriesViewModel> RenderableSeries { get; private set; }

        public DateRange XVisibleRangeLimit { get; }

        public DateRange XVisibleRange
        {
            get => _xVisibleRange;
            set
            {
                _xVisibleRange = value;
                OnPropertyChanged(nameof(XVisibleRange));
            }
        }

        public DateTime XCurrentDate
        {
            get => _xCurrentDate;
            set
            {
                if (_xCurrentDate != value)
                {
                    _xCurrentDate = value;
                    OnPropertyChanged(nameof(XCurrentDate));
                    Items.ForEachDo(x => x.CheckCompletion(XCurrentDate));
                }
            }
        }

        public DateTime XEndDate
        {
            get => _xEndDate;
            set
            {
                if (_xEndDate != value)
                {
                    _xEndDate = value;
                    OnPropertyChanged(nameof(XEndDate));
                }
            }
        }

        public GanttChartViewModel()
        {
            Items = new[]
            {
                new GanttItemViewModel(1)
                {
                    Name = "New Product Strategy",
                    Department = "Marketing/Business",
                    Start = new DateTime(2022, 05, 10),
                    End = new DateTime(2022, 06, 10),
                    Color = Color.FromRgb(255, 95, 100)
                },

                new GanttItemViewModel(2)
                {
                    Name = "Idea Generation",
                    Department = "Marketing/Engineering",
                    Start = new DateTime(2022, 05, 10),
                    End = new DateTime(2022, 06, 30),
                    Color = Color.FromRgb(255, 150, 70)
                },

                new GanttItemViewModel(3)
                {
                    Name = "Screening",
                    Department = "Marketing",
                    Start = new DateTime(2022, 06, 30),
                    End = new DateTime(2022, 07, 25),
                    Color = Color.FromRgb(245, 185, 50)
                },

                new GanttItemViewModel(4)
                {
                    Name = "Concept Testing",
                    Department = "Engineering",
                    Start = new DateTime(2022, 07, 25),
                    End = new DateTime(2022, 09, 02),
                    Color = Color.FromRgb(35, 225, 130)
                },

                new GanttItemViewModel(5)
                {
                    Name = "Business Analysis",
                    Department = "Business",
                    Start = new DateTime(2022, 07, 25),
                    End = new DateTime(2022, 10, 10),
                    Color = Color.FromRgb(45, 205, 185)
                },

                new GanttItemViewModel(6)
                {
                    Name = "Product Development",
                    Department = "Engineering",
                    Start = new DateTime(2022, 09, 12),
                    End = new DateTime(2023, 01, 20),
                    Color = Color.FromRgb(70, 170, 240)
                },

                new GanttItemViewModel(7)
                {
                    Name = "Market Testing",
                    Department = "Marketing",
                    Start = new DateTime(2022, 11, 01),
                    End = new DateTime(2023, 02, 14),
                    Color = Color.FromRgb(75, 125, 235)
                },

                new GanttItemViewModel(8)
                {
                    Name = "Focus Group Testing",
                    Department = "Marketing",
                    Start = new DateTime(2022, 12, 30),
                    End = new DateTime(2023, 02, 14),
                    Color = Color.FromRgb(165, 95, 235)
                },

                new GanttItemViewModel(9)
                {
                    Name = "Authorization",
                    Department = "Business/Engineering",
                    Start = new DateTime(2023, 01, 20),
                    End = new DateTime(2023, 02, 14),
                    Color = Color.FromRgb(235, 65, 145)
                },

                new GanttItemViewModel(10)
                {
                    Name = "Commercialization",
                    Department = "Business",
                    Start = new DateTime(2023, 01, 20),
                    End = new DateTime(2023, 03, 10),
                    Color = Color.FromRgb(215, 50, 50)
                },

                new GanttItemViewModel(11)
                {
                    Name = "Product Pricing",
                    Department = "Business/Marketing",
                    Start = new DateTime(2023, 02, 15),
                    End = new DateTime(2023, 03, 10),
                    Color = Color.FromRgb(240, 90, 35)
                },

                new GanttItemViewModel(12)
                {
                    Name = "Product Licensing",
                    Department = "Business",
                    Start = new DateTime(2023, 02, 15),
                    End = new DateTime(2023, 03, 28),
                    Color = Color.FromRgb(255, 195, 20)
                },

                new GanttItemViewModel(13)
                {
                    Name = "Promotion",
                    Department = "Marketing",
                    Start = new DateTime(2023, 03, 10),
                    End = new DateTime(2023, 04, 10),
                    Color = Color.FromRgb(40, 205, 195)
                },

                new GanttItemViewModel(14)
                {
                    Name = "Launch",
                    Department = "Business",
                    Start = new DateTime(2023, 03, 28),
                    End = new DateTime(2023, 04, 15),
                    Color = Color.FromRgb(125, 215, 55)
                }
            };

            XVisibleRange = new DateRange
            {
                Min = new DateTime(2022, 05, 03),
                Max = new DateTime(2023, 02, 23)
            };

            XVisibleRangeLimit = new DateRange
            {
                Min = new DateTime(2022, 02, 01),
                Max = new DateTime(2023, 07, 31)
            };

            XCurrentDate = new DateTime(2022, 07, 04);
            XEndDate = new DateTime(2023, 04, 17);

            CreateYAxes();
            CreateRenderableSeries();
        }

        private void CreateYAxes()
        {
            if (YAxes == null)
            {
                YAxes = new List<IAxisViewModel>
                {
                    new NumericAxisViewModel
                    {
                        AxisAlignment = AxisAlignment.Right,
                        StyleKey = "DefaultYAxisStyle"
                    }
                };

                Items.OrderByDescending(x => x.Id).ForEachDo(item =>
                {
                    YAxes.Add(new NumericAxisViewModel
                    {
                        Id = $"YAxis-{item.Id}",
                        AxisAlignment = AxisAlignment.Left,
                        StyleKey = "ItemYAxisStyle"
                    });
                });
            }
        }

        private void CreateRenderableSeries()
        {
            if (RenderableSeries == null)
            {
                RenderableSeries = new List<IRenderableSeriesViewModel>();

                Items.OrderByDescending(x => x.Id).ForEachDo(item =>
                {
                    RenderableSeries.Add(new StripeRenderableSeriesViewModel
                    {
                        YAxisId = $"YAxis-{item.Id}",
                        Stroke = item.Color,
                        Fill = item.Fill,
                        DataSeries = new StripeDataSeries<DateTime, double>(new[] { item.Start }, new[] { item.End }, 0d, 1d),
                        PointLabelProvider = new GanttTextLabelProvider(item),
                        StyleKey = "ItemRenderableSeriesStyle"
                    });
                });
            }
        }
    }
}