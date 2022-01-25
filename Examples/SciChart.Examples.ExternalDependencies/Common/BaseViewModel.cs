// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BaseViewModel.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.ComponentModel;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;

#if SILVERLIGHT
        private static Dispatcher _dispatcher;
        public BaseViewModel()
        {
            TryGetDispatcher();
        }

        private void TryGetDispatcher()
        {
            try
            {
                if (Application.Current.RootVisual != null && _dispatcher == null)
                    _dispatcher = Application.Current.RootVisual.Dispatcher;
            }
            catch
            {
            }
        }
#endif

        protected void OnPropertyChanged(string propertyName)
        {            
#if SILVERLIGHT
            if (_dispatcher == null) TryGetDispatcher();

            if (_dispatcher != null && !_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(() => OnPropertyChanged(propertyName));
                return;
            }
#endif            

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}