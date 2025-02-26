// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomExportModifier.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals;
using SciChart.Core;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers
{
    public class CustomExportModifier : ChartModifierBase
    {
        public ICommand SavePngCommand { get; }
        public ICommand SaveSizePngCommand { get; }

        public ICommand SaveXpsCommand { get; }
        public ICommand SaveSizeXpsCommand { get; }

        public bool UseXamlRs { get; set; }

        public CustomExportModifier()
        {
            SavePngCommand = new ActionCommand(() => Export("PNG|*.png", UseXamlRs));
            SaveSizePngCommand = new ActionCommand(() => Export("PNG|*.png", UseXamlRs, new Size(2000, 1000)));

            SaveXpsCommand = new ActionCommand(() => Export("XPS|*.xps", UseXamlRs));
            SaveSizeXpsCommand = new ActionCommand(() => Export("XPS|*.xps", UseXamlRs, new Size(2000, 1000)));
        }

        private void Export(string filter, bool useXaml, Size? size = null)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (saveFileDialog.ShowDialog() == true && ParentSurface is SciChartSurface scs)
            {
                var exportType = filter.ToUpper().Contains("XPS") ? ExportType.Xps : ExportType.Png;

                if (size.HasValue)
                {
                    scs.ExportToFile(saveFileDialog.FileName, exportType, useXaml, size.Value);
                }
                else
                {
                    scs.ExportToFile(saveFileDialog.FileName, exportType, useXaml);
                }

                try
                {
                    Process.Start(saveFileDialog.FileName);
                }
                catch (Win32Exception e)
                {
                    if (e.NativeErrorCode == 1155)
                    {
                        MessageBox.Show("Can't open because no application is associated with the specified file for this operation.", "Exported successfully!");
                    }
                }
            }
        }
    }
}