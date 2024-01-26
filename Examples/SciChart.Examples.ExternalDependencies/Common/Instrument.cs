// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Instrument.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Globalization;
using System.Linq;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class Instrument : StrongTyped<string>
    {
        public string InstrumentName { get; private set; }
        public string Symbol { get { return Value; } }
        public int DecimalPlaces { get; private set; }

        public Instrument(string value, string instrumentName, int decimalPlaces) : base(value)
        {
            InstrumentName = instrumentName;
            DecimalPlaces = decimalPlaces;
        }

        public static Instrument Parse(string instrumentString)
        {
            return new[] { EurUsd, Indu, Spx500, CrudeOil, Test }.Single(x => x.Symbol.ToUpper(CultureInfo.InvariantCulture) == instrumentString.ToUpper(CultureInfo.InvariantCulture));
        }

        public static readonly Instrument EurUsd = new Instrument("EURUSD", "FX Euro US Dollar", 4);
        public static readonly Instrument Indu = new Instrument("INDU", "Dow Jones Industrial Average", 0);
        public static readonly Instrument Spx500 = new Instrument("SPX500", "S&P500 Index", 0);
        public static readonly Instrument CrudeOil = new Instrument("CL", "Light Crude Oil", 0);
        public static readonly Instrument Test = new Instrument("TEST", "Test data only", 0);
    }
}