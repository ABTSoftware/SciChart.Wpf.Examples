// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BaseViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.ComponentModel;

namespace SciChart.Examples.ExternalDependencies.Common
{
    /// <summary>
    /// A BaseViewModel class used in the SciChart WPF Examples suite
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
    {        
        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event on the view-model property.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {            
            var handler = PropertyChanged;
            
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));          
        }

        /// <summary>
        /// Called by the SciChart Examples Framework when an example is unloaded. Used to de-initialize memory, timers etc.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Called by the SciChart Examples Framework when an example is unloaded. Used to de-initialize memory, timers etc.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor to de-initialize memory, timers etc.
        /// </summary>
        ~BaseViewModel()
        {
            Dispose(false);
        }
    }
}