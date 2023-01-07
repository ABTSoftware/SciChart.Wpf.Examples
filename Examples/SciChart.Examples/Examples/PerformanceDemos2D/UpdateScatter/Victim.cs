// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Victim.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Media;

namespace SciChart.Examples.Examples.PerformanceDemos2D.UpdateScatter
{
    public sealed class Victim : Person
    {
        public Victim(Point initialPos)
            : base(initialPos)
        {
            PersonId = "V";
            ShowColor = new SolidColorBrush(Colors.Green);
        }

        public Person Aggressor { get; set; }
        public Person Defender { get; set; }

        public override void Initialize(World world)
        {
            Aggressor = world.GetRandomOtherPerson(this);
            Defender = world.GetRandomOtherPerson(this, Aggressor);
        }

        public sealed override void CalculateTarget()
        {            
            Point aggressorPos = Aggressor.Pos;
            Point defenderPos = Defender.Pos;
            Point threatAxis = new Point((aggressorPos.X - Pos.X), (aggressorPos.Y - Pos.Y));
            Point flightAxis = new Point(-threatAxis.Y, threatAxis.X); // 90 degrees to Threat        

            double length = Math.Sqrt(flightAxis.X * flightAxis.X + flightAxis.Y * flightAxis.Y);
            if (length == 0 || defenderPos == aggressorPos)
            {
                Target = defenderPos;
            }
            else
            {
                Point safeVector = new Point((defenderPos.X - aggressorPos.X), (defenderPos.Y - aggressorPos.Y));
                double denom = (flightAxis.X*safeVector.Y - flightAxis.Y*safeVector.X);
                double q = denom == 0 ? 1 : (safeVector.X*threatAxis.Y - safeVector.Y*threatAxis.X)/denom;
                if (q < 1 || double.IsNaN(q))
                {
                    q = 1; // Make sure we're at least *behind* the defender, rather than just in line
                }
                Target = new Point(Pos.X + q * flightAxis.X, Pos.Y + q*flightAxis.Y);
            }
        }

        public override bool IsVictim
        {
            get { return true; }
        }
    }
}