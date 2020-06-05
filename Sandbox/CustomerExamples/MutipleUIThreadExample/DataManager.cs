using SciChart.Charting.Model.DataSeries;

namespace MutipleUIThreadExample
{
    public enum Category
    {
        Meat,
        Vegetables
    }

    public class DataManager
    {
        public XyDataSeries<double> DataSeriesPork { get; }
        public XyDataSeries<double> DataSeriesVeal { get; }
        public XyDataSeries<double> DataSeriesTomato { get; }
        public XyDataSeries<double> DataSeriesCucumber { get; }
        public XyDataSeries<double> DataSeriesPepper { get; }

        public DataManager()
        {
            const int StartDate = 1992;

            var porkData = new double[] { 10, 13, 7, 16, 4, 6, 20, 14, 16, 10, 24, 11 };
            var vealData = new double[] { 12, 17, 21, 15, 19, 18, 13, 21, 22, 20, 5, 10 };
            var tomatoesData = new double[] { 7, 30, 27, 24, 21, 15, 17, 26, 22, 28, 21, 22 };
            var cucumberData = new double[] { 16, 10, 9, 8, 22, 14, 12, 27, 25, 23, 17, 17 };
            var pepperData = new double[] { 7, 24, 21, 11, 19, 17, 14, 27, 26, 22, 28, 16 };

            DataSeriesPork = new XyDataSeries<double> { SeriesName = "Pork" };
            DataSeriesVeal = new XyDataSeries<double> { SeriesName = "Veal" };
            DataSeriesTomato = new XyDataSeries<double> { SeriesName = "Tomato" };
            DataSeriesCucumber = new XyDataSeries<double> { SeriesName = "Cucumber" };
            DataSeriesPepper = new XyDataSeries<double> { SeriesName = "Pepper" };

            for (int i = 0; i < porkData.Length; i++)
            {
                DataSeriesPork.Append(StartDate + i, porkData[i]);
                DataSeriesVeal.Append(StartDate + i, vealData[i]);
                DataSeriesTomato.Append(StartDate + i, tomatoesData[i]);
                DataSeriesCucumber.Append(StartDate + i, cucumberData[i]);
                DataSeriesPepper.Append(StartDate + i, pepperData[i]);
            }
        }

        public XyDataSeries<double> AggregateByCategory(Category category)
        {
            var dataSeries = new XyDataSeries<double> { SeriesName = category.ToString() };

            if (category == Category.Meat)
            {
                for (int i = 0; i < DataSeriesPork.Count; i++)
                {
                    dataSeries.Append(DataSeriesPork.XValues[i], 
                        DataSeriesPork.YValues[i] + DataSeriesVeal.YValues[i]);
                }
            }
            else if (category == Category.Vegetables)
            {
                for (int i = 0; i < DataSeriesTomato.Count; i++)
                {
                    dataSeries.Append(DataSeriesTomato.XValues[i],
                        DataSeriesTomato.YValues[i] + DataSeriesCucumber.YValues[i] + DataSeriesPepper.YValues[i]);
                }
            }
            return dataSeries;
        }
    }
}