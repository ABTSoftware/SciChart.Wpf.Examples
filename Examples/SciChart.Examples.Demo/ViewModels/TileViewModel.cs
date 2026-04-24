// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// TileViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Examples.Demo.Controls.Tile;
using SciChart.Examples.Demo.Helpers;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Demo.ViewModels
{
    public class TileViewModel : BaseViewModel
    {
        private static readonly Random Random = new Random();

        private TileState _tileState;

        public TileViewModel()
        {
            TransitionSeed = Random.Next(1, 10) * 3;
            TransitionTime = TimeSpan.FromSeconds(Random.Next(6, 22) * 2 / 10d);
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