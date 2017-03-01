using System;
using SciChart.Examples.Demo.Controls.TileControl;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Demo.ViewModels
{
    public class TileViewModel :BaseViewModel
    {
        private static readonly Random Random = new Random();

        private TileState _tileState;

        public TileViewModel()
        {
            TransitionSeed = Random.Next(1, 10) * 3;
            TransitionTime = TimeSpan.FromSeconds(Random.Next(6, 22)*2 / 10d);
        }

        public TileState TileState
        {
            get { return _tileState; }
            set
            {
                _tileState = value;
                OnPropertyChanged("TileState");
            }
        }

        public int TransitionSeed { get; set; }

        public TimeSpan TransitionTime { get; set; }

        public ISelectable TileDataContext { get; set; }

        public void ChangeState()
        {
            switch (TileState)
            {
                case TileState.Main:
                    TileState = TileState.Details;
                    break;
                case TileState.Details:
                    TileState = TileState.Main;
                    break;
            }
        }
    }
}