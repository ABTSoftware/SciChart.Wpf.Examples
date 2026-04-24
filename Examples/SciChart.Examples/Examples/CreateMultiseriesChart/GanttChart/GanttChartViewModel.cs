// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// GanttChartViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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
    /// <summary>
    /// Main view-model for the Gantt Chart example.
    ///
    /// Architecture overview:
    ///   The chart is built from a single <c>SciChartSurface</c> that uses one hidden X-axis shared
    ///   with a separate header surface, and a collection of stacked Y-axes — one per task row.
    ///   Each task row also gets one <c>StripeRenderableSeries</c> whose horizontal extent represents
    ///   the task's start-to-end date range.
    ///
    ///   A draggable green vertical line (<c>VerticalSliceModifier</c>) represents the current date
    ///   and drives completion recalculation for all items via <see cref="XCurrentDate"/>.
    /// </summary>
    public class GanttChartViewModel : BaseViewModel
    {
        private DateRange _xVisibleRange;
        private DateTime _xCurrentDate;
        private DateTime _xEndDate;

        /// <summary>Task data bound to the left-panel list and used to generate axes/series.</summary>
        public IList<GanttItemViewModel> Items { get; }

        /// <summary>
        /// One <c>NumericAxis</c> per task row, plus a hidden default right-aligned axis.
        /// Bound to <c>SciChartSurface.YAxes</c>.
        /// </summary>
        public IList<IAxisViewModel> YAxes { get; private set; }

        /// <summary>
        /// One <c>StripeRenderableSeriesViewModel</c> per task row.
        /// Bound to <c>SciChartSurface.RenderableSeries</c>.
        /// </summary>
        public IList<IRenderableSeriesViewModel> RenderableSeries { get; private set; }

        /// <summary>Hard limits on how far the user can pan or zoom the timeline.</summary>
        public DateRange XVisibleRangeLimit { get; }

        /// <summary>
        /// The currently visible date range, two-way bound to both the header and chart X-axes
        /// so they scroll in lockstep.
        /// </summary>
        public DateRange XVisibleRange
        {
            get => _xVisibleRange;
            set
            {
                _xVisibleRange = value;
                OnPropertyChanged(nameof(XVisibleRange));
            }
        }

        /// <summary>
        /// The date represented by the draggable green marker. Changing this recalculates the
        /// completion percentage for every task.
        /// </summary>
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

        /// <summary>The project end date, shown as a fixed orange vertical line annotation.</summary>
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

        /// <summary>
        /// Builds the Y-axis collection: one per-task axis on the left in reverse order.
        /// Reversing the item order means the first task ends up at the top of the stacked panel.
        /// Each axis is styled to zero width and a fixed visible range so it acts purely as a
        /// layout row with no tick marks or labels of its own.
        /// </summary>
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

                // Add one Y-axis per item in descending order so item 1 renders at the top.
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

        /// <summary>
        /// Builds one <c>StripeRenderableSeriesViewModel</c> per task, in the same descending order
        /// as <see cref="CreateYAxes"/>. Each series is assigned to its matching Y-axis by ID.
        ///
        /// The <c>StripeDataSeries</c> encodes the task bar as a single stripe:
        ///   X = Start date, X1 = End date, Y = 0 (bottom of the bar), Y1 = 1 (top of the bar).
        ///
        /// A <see cref="GanttTextLabelProvider"/> draws the working-day count inside each bar.
        /// </summary>
        private void CreateRenderableSeries()
        {
            if (RenderableSeries == null)
            {
                RenderableSeries = new List<IRenderableSeriesViewModel>();

                // Match the axis ordering so each series is paired with the correct row.
                Items.OrderByDescending(x => x.Id).ForEachDo(item =>
                {
                    RenderableSeries.Add(new StripeRenderableSeriesViewModel
                    {
                        YAxisId = $"YAxis-{item.Id}",
                        Stroke = item.Color,
                        Fill = item.Fill,
                        // Single stripe: X=start, X1=end, Y=0..1 spans the full row height.
                        DataSeries = new StripeDataSeries<DateTime, double>(new[] { item.Start }, new[] { item.End }, 0d, 1d),
                        PointLabelProvider = new GanttTextLabelProvider(item),
                        StyleKey = "ItemRenderableSeriesStyle"
                    });
                });
            }
        }
    }
}