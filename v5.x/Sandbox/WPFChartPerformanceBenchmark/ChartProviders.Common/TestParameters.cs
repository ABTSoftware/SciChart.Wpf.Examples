using System.Windows.Input;

namespace ChartProviders.Common
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

    //Arction 
    public enum TestRunnerType
    {
        DispatcherTimer=0,
        Composition = 1,
         
    }

	public enum DataDistribution
	{
		Uniform,
		ConcentratedAlongLine
	}

    public class TestParameters
    {
        public TestParameters(TestRunnerType runner, int pointCount )
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

      


		public int PointCount { get; set; }
		public int PointCountThreshold { get; set; } // when total points count goes over PointCountThreshold, it starts to append PointCount2 per frame. this avoids OutOfMemory exception
		public int PointCount2 { get; set; }
		public Resampling SamplingMode { get; set; }
		public DataDistribution DataDistribution { get; set; }
        public bool AntiAliasing { get; set; }
        public float StrokeThickness { get; set; }
        public TestRunnerType TestRunner { get; set; } 

        public override string ToString()
        {
			return string.Format("{0} pts, AA={1}, StrokeThickness={2}, Resampling={3}, Distr={4}, TestRunner={5}", PointCount, AntiAliasing ? "On" : "Off", StrokeThickness, SamplingMode, DataDistribution,TestRunner.ToString());
        }
    }
}