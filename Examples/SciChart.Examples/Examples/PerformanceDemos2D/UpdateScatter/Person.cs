// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Person.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using SciChart.Drawing.Utility;

namespace SciChart.Examples.Examples.PerformanceDemos2D.UpdateScatter
{
    public abstract class Person : INotifyPropertyChanged
    {
        private Point _pos;
        public string PersonId { get; protected set; }

        public Brush ShowColor { get; protected set; }

        public Point Pos
        {
            get { return _pos; }
            protected set
            {
                _pos = value;
                PropChanged("Pos");
            }
        }

        public Point Target { get; protected set; }

        public const double Speed = 1;

        protected Person(Point initialPos)
        {
            Pos = initialPos;
        }

        public abstract void Initialize(World world);
        public abstract void CalculateTarget();

        public abstract bool IsVictim { get; }

        private static double To01(double arg)
        {
            if (arg < 0)
                return 0;
            if (arg > 1)
                return 1;
            return arg;
        }

        public void UpdatePosition(double deltaT)
        {
            //Constrain target to (0,0)-(1,1)
            Point target2 = new Point(To01(Target.X), To01(Target.Y));
            double distance = PointUtil.Distance(target2, Pos);
            if (distance < Speed*deltaT)
            {
                Pos = target2;
            }
            else
            {
                // Get the velocity vector
                Point velocity = new Point((target2.X - Pos.X), (target2.Y - Pos.Y));

                // Normalize the vector
                double length = Math.Sqrt(velocity.X*velocity.X + velocity.Y*velocity.Y);
                velocity = new Point(velocity.X / length, velocity.Y / length);

                // Compute updated position 
                var newPos = new Point(Pos.X + velocity.X*deltaT, Pos.Y + velocity.Y*deltaT);
                Pos = newPos;
            }
        }

        private void PropChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}