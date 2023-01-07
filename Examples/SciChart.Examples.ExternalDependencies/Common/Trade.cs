// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Trade.cs is part of SCICHART®, High Performance Scientific Charts
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
using System;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class Trade
    {
        public Instrument Instrument { get; set; }
        public BuySell BuySell { get; set; }
        public double DealPrice { get; set; }
        public double TotalPrice { get { return DealPrice*Quantity; } }
        public DateTime TradeDate { get; set; }
        public int Quantity { get; set; }
    }

    public enum BuySell
    {
        Buy = 1,
        Sell = -1
    }
}