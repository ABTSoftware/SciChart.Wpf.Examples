// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomChangeThemeModifier.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals;
using SciChart.Core;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar.CustomModifiers
{
    public class CustomExportModifier : ChartModifierBase
    {
        private bool _useXamlRs;

        public ICommand SavePngCommand { get { return new ActionCommand(this.SavePng); } }

        public ICommand SavePngBigCommand { get {  return new ActionCommand(this.SavePngBig);} }

        public ICommand SaveXpsCommand { get {  return new ActionCommand(this.SaveXps);} }

        public ICommand SaveXpsBigCommand { get {  return new ActionCommand(this.SaveXpsBig);} }

        public bool UseXamlRs
        {
            get { return _useXamlRs; }
            set
            {
                _useXamlRs = value;
            }
        }

        private void SavePng()
        {
            Export("Png|*.png", UseXamlRs, null);
        }

        private void SavePngBig()
        {
            Export("Png|*.png", UseXamlRs, new Size(2000, 1000));
        }

        private void SaveXps()
        {
            Export("XPS|*.xps", UseXamlRs, null);
        }

        private void SaveXpsBig()
        {
            Export("XPS|*.xps", UseXamlRs, new Size(2000, 1000));
        }

        private void Export(string filter, bool useXaml, System.Windows.Size? size = null)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var surface = ParentSurface as SciChartSurface;
                if (surface == null)
                    return;

                var exportType = filter.ToUpper().Contains("XPS") ? ExportType.Xps : ExportType.Png;

                if (size.HasValue)
                {
                    surface.ExportToFile(saveFileDialog.FileName, exportType, useXaml, size.Value);
                }
                else
                {
                    surface.ExportToFile(saveFileDialog.FileName, exportType, useXaml);
                }

                Process.Start(saveFileDialog.FileName);
            }
        }
    }

    public class CustomThemeChangeModifier : ChartModifierBase
    {
        public string SelectedTheme
        {
            get
            {
                var theme = ThemeManager.GetTheme((SciChartSurface)ParentSurface);

                return string.IsNullOrEmpty(theme) ? ThemeManager.DefaultTheme : theme;
            }
            set
            {
                ThemeManager.SetTheme((SciChartSurface)ParentSurface, value);
            }
        }
    }
}
