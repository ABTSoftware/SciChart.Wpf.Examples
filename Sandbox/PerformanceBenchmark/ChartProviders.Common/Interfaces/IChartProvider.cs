namespace ChartProviders.Common.Interfaces
{
    public interface IChartProvider
    {
        string Name { get; }

        ISpeedTest ScatterPointsSpeedTest();
        ISpeedTest FifoLineSpeedTest();
        ISpeedTest LineAppendSpeedTest();
        ISpeedTest LoadNxNRefreshTest();        
    }
}