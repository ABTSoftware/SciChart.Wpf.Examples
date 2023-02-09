namespace SciChart.Examples.ExternalDependencies.Data
{
    public enum ContinentsEnum
    {
        Asia,
        Europe,
        Africa,
        Americas,
        Oceania,
    }

    public class PopulationData
    {
        public string Country { get; set; }
        public int Year { get; set; }
        public int Population { get; set; }
        public ContinentsEnum Continent { get; set; }
        public double LifeExpectancy { get; set; }
        public double GDPPerCapita { get; set; }
    }
}
