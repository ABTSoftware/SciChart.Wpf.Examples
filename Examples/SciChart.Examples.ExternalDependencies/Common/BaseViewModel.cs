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