namespace ChartProviders.Common.Models
{
    public class LineAppendTestParameters : TestParameters
    {
        public int IncrementPoints { get; set; }

        public double Noisyness { get; set; }

        public LineAppendTestParameters(TestRunnerType runner, int startingPoints, int incrementPoints, double noisyness, bool aa, Resampling resampling)
            : base(runner, startingPoints, resampling, aa)
        {
            IncrementPoints = incrementPoints;
            Noisyness = noisyness;          
        }

        public LineAppendTestParameters(TestRunnerType runner, int pointCount) : base(runner, pointCount)
        {          
        }

        public override string ToString()
        {
            return $"{PointCount}+{IncrementPoints} pts, AA={AntiAliasing}, StrokeThickness={StrokeThickness}, Resampling={SamplingMode}, Noisyness={Noisyness}, Runner={TestRunner}";
        }
    }
}