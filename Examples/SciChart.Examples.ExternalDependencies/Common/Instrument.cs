// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// Instrument.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
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