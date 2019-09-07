using System;
using System.Collections.Generic;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Data.Numerics;
using SciChart.Data.Numerics.PointResamplers;

namespace Fifo100MillionPointsDemo.HelperClasses
{
    public class StreamingSeries<TX,TY> : XyDataSeries<TX,TY>
        where TX:IComparable
        where TY:IComparable
    {
        public override void Append(IEnumerable<TX> x, IEnumerable<TY> y)
        {
            base.Append(x, y);
        }

        protected override IPointSeries ToPointSeriesInternal(ResamplingParams resamplingParams, ResamplingMode resamplingMode,
            IPointResamplerFactory factory, IPointSeries lastPointSeries)
        {
            return base.ToPointSeriesInternal(resamplingParams, resamplingMode, factory, lastPointSeries);
        }
    }
}