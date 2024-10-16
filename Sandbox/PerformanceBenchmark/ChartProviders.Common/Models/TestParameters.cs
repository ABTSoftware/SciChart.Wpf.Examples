namespace ChartProviders.Common.Models
{
    public enum Resampling
    {
        None,
        MinMax,
        MinMaxWithUnevenSpacingNative,
        MinMaxNative,
        MinMaxWithUnevenSpacing,
        Cluster2D,
        Cluster2DInRenderableSeries,
        Auto
    }

    public enum TestRunnerType
    {
        DispatcherTimer,
        Composition
    }

    public enum DataDistribution
    {
        Uniform,
        ConcentratedAlongLine
    }

    public class TestParameters
    {
        public int PointCount { get; set; }
        public int PointCount2 { get; set; }

        // When total points count goes over PointCountThreshold, it starts to append PointCount2 per frame.
        // This avoids OutOfMemory exception.
        public int PointCountThreshold { get; set; }

        public Resampling SamplingMode { get; set; }
        public DataDistribution DataDistribution { get; set; }
        public TestRunnerType TestRunner { get; set; }

        public bool AntiAliasing { get; set; }
        public float StrokeThickness { get; set; }

        public TestParameters(TestRunnerType runner, int pointCount)
        {
            PointCount = pointCount;
            SamplingMode = Resampling.None;
            AntiAliasing = false;
            StrokeThickness = 1.0f;
            TestRunner = runner;
        }

        public TestParameters(TestRunnerType runner, int pointCount, Resampling resampling, bool antialiasing,
            DataDistribution dataDistribution = DataDistribution.Uniform, int pointsCountThreshold = 100000000, int pointsCount2 = 0)
        {
            DataDistribution = dataDistribution;
            PointCount = pointCount;
            PointCountThreshold = pointsCountThreshold;
            PointCount2 = pointsCount2;
            SamplingMode = resampling;
            AntiAliasing = antialiasing;
            TestRunner = runner;
        }

        public override string ToString()
        {
            return $"{PointCount} pts, AA={AntiAliasing}, StrokeThickness={StrokeThickness}, Resampling={SamplingMode}, Distr={DataDistribution}, TestRunner={TestRunner}";
        }
    }
}