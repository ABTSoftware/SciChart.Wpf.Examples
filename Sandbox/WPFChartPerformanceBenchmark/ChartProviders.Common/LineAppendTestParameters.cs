namespace ChartProviders.Common
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
            return string.Format("{0}+{1} pts, AA={2}, StrokeThickness={3}, Resampling={4}, Noisyness={5}, Runner={6}", PointCount, IncrementPoints, AntiAliasing ? "On" : "Off", StrokeThickness, SamplingMode, Noisyness,TestRunner.ToString());
        }
    }
}