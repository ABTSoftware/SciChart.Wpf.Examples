using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.StyleAChart.UseLabelProvider
{
    public partial class UseLabelProvider : UserControl
    {
        public UseLabelProvider()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            xAxis.LabelProvider = new CustomLabelProvider(new CustomLabelParams
            {
                ColorFrom = Colors.GreenYellow,
                ColorTo = Colors.DodgerBlue,

                ValueFrom = 0,
                ValueTo = 10,

                LabelAngle = 0,
                LabelFormat = "X{0}"
            });

            yAxis.LabelProvider = new CustomLabelProvider(new CustomLabelParams
            {
                ColorFrom = Colors.OrangeRed,
                ColorTo = Colors.BlueViolet,

                ValueFrom = 0,
                ValueTo = 10,

                LabelAngle = -30,
                LabelFormat = "Y{0:N2}"
            });

            var dataSeries = new UniformXyDataSeries<double>();
            var yValues = new[] {8, 6, 10, 7, 8, 10, 9, 8, 7, 8, 10};

            using (sciChartSurface.SuspendUpdates())
            {
                for (int i = 0; i < yValues.Length; i++)
                {
                    dataSeries.Append(yValues[i]);
                }
            }

            columnSeries.DataSeries = dataSeries;
            sciChartSurface.ZoomExtents();
        }
    }
}