// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// World.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SciChart.Examples.Examples.PerformanceDemos2D.UpdateScatter
{
    public class World : INotifyPropertyChanged
    {
        public delegate void WorldTickHandler(object sender, EventArgs e);

        private Random _rng;
        private double _deltaT;

        private int _population;
        private int _seed;
        private double _victimFraction;

        public World(int population = 10000)
        {
            Seed = 1;
            Population = population;
            VictimFraction = 0.9;
            _deltaT = 0.01;
        }

        public Person[] People { get; private set; }

        // To use when repopulating, not necessarily current now
        public int Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                PropChanged("Seed");
            }
        }

        public int Population
        {
            get { return _population; }
            set
            {
                _population = value;
                PropChanged("Population");
            }
        }

        public double VictimFraction
        {
            get { return _victimFraction; }
            set
            {
                _victimFraction = value;
                PropChanged("VictimFraction");
            }
        }

        public double DeltaT
        {
            get { return _deltaT; }
            set
            {
                _deltaT = value;
                PropChanged("DeltaT");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event WorldTickHandler OnWorldTick = delegate { };

        public void Test()
        {
            var v = new Victim(new Point(0.8, 0.3));
            var a = new Victim(new Point(0.8, 0.8));
            var d = new Victim(new Point(0.5, 0.5));
            v.Aggressor = a;
            v.Defender = d;
            v.CalculateTarget();
            //v.Target should be 0.3,0.3
        }


        public double NextRandomDouble()
        {
            lock (_rng)
            {
                return _rng.NextDouble();
            }
        }

        public int NextRandomIndex()
        {
            lock (_rng)
            {
                return _rng.Next(People.Length);
            }
        }

        public void Populate()
        {
            _rng = new Random(Seed);
            People = new Person[Population];

#if !SILVERLIGHT 
            Parallel.For(0, People.Length, i =>
                {
                    var pos = new Point(NextRandomDouble(), NextRandomDouble());
                    People[i] = NextRandomDouble() < VictimFraction ? (Person) new Victim(pos) : new Defender(pos);
                });
#else
            for (int i = 0; i < People.Length; i++)
            {
                var pos = new Point(NextRandomDouble(), NextRandomDouble());
                People[i] = NextRandomDouble() < VictimFraction ? (Person)new Victim(pos) : new Defender(pos);
            }
#endif

            for (int i = 0; i < People.Length; ++i)
            {
                People[i].Initialize(this);
            }
            PropChanged("People");
        }

        public Person GetRandomOtherPerson(params Person[] notThese)
        {
            Person result;
            do
            {
                int i = NextRandomIndex();
                result = People[i];
            } while (notThese.Contains(result));
            return result;
        }

        public void Tick(int cpuCount = 2)
        {
            if (People != null)
            {
                int chunkSize = People.Length/cpuCount;
                Parallel.For(0, cpuCount, cpu =>
                    {
                        int nextChunk = cpu*chunkSize + chunkSize;
                        for (int i = cpu*chunkSize; i < nextChunk; i++)
                        {
                            Person person = People[i];
                            person.CalculateTarget();
                            person.UpdatePosition(_deltaT);
                        }
                    });      


                if (OnWorldTick != null)
                {
                    OnWorldTick(this, EventArgs.Empty);
                }
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
    }
}