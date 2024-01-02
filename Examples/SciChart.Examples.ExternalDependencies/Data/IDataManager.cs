// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// IDataManager.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System.Collections.Generic;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public interface IDataManager
    {
        PriceSeries GetPriceData(string symbol, TimeFrame timeFrame);
        DoubleSeries GetFourierSeries(double amplitude, double phaseShift, int count=5000);
        DoubleSeries GetSquirlyWave();

        IEnumerable<Instrument> AvailableInstruments { get; }
        IEnumerable<TimeFrame> GetAvailableTimeFrames(Instrument forInstrument);        
    }
}