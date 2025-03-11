// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Defender.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using System.Windows.Media;

namespace SciChart.Examples.Examples.PerformanceDemos2D.UpdateScatter
{
    public class Defender : Person
    {
        public Defender(Point initialPos)
            : base(initialPos)
        {
            PersonId = "D";
            ShowColor = new SolidColorBrush(Colors.Red);
        }

        public Person Aggressor { get; private set; }
        public Person Victim { get; private set; }

        public override void Initialize(World world)
        {
            Aggressor = world.GetRandomOtherPerson(this);
            Victim = world.GetRandomOtherPerson(this, Aggressor);
        }


        public override void CalculateTarget()
        {
            Point diff = new Point((Victim.Pos.X - Aggressor.Pos.X)* 0.5, (Victim.Pos.Y - Aggressor.Pos.Y) * 0.5);
            Target = new Point(Aggressor.Pos.X + diff.X, Aggressor.Pos.Y + diff.Y);
        }

        public override bool IsVictim
        {
            get { return false; }
        }
    }
}