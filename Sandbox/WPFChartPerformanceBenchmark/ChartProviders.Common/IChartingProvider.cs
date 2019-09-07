namespace ChartProviders.Common
{
    public interface IChartingProvider
    {
        ISpeedTest ScatterPointsSpeedTest();
        ISpeedTest FifoLineSpeedTest();
        ISpeedTest LineAppendSpeedTest();
        ISpeedTest LoadNxNRefreshTest();

        string Name { get; }
        
    }
}
