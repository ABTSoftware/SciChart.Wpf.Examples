using System.Collections.Generic;
using System.Linq;
namespace SciChart.Sandbox.Shared
{
    /// <summary>
    /// A data-structure to contain a list of X,Y double-precision points
    /// </summary>
    public class DoubleSeries : List<XYPoint>
    {
        public DoubleSeries()
        {
        }

        public DoubleSeries(int capacity)
            : base(capacity)
        {
        }

        public IList<double> XData { get { return this.Select(x => x.X).ToArray(); } }
        public IList<double> YData { get { return this.Select(x => x.Y).ToArray(); } }
    }
}