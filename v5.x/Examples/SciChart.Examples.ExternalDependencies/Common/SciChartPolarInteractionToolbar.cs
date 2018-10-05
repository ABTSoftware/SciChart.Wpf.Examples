// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SciChartPolarInteractionToolbar.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Windows.Data;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals;
using SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar;

namespace SciChart.Examples.ExternalDependencies.Common
{
    /// <summary>
    /// A toolbar used in examples to simplify zoom, oom extents, rollover, cursor etc... for the polar charts. This also helps us with testing ;)
    /// </summary>
    public class SciChartPolarInteractionToolbar : SciChartInteractionToolbar
    {
        public SciChartPolarInteractionToolbar()
        {
            DefaultStyleKey = typeof(SciChartPolarInteractionToolbar);
        }

        protected override void OnCreateModifiers(SciChartInteractionToolbar toolbar, ISciChartSurface scs)
        {
            var rbzm = new RubberBandXyZoomModifier { IsXAxisOnly = false };
            var mouseWheelZoomPanModifier = new MouseWheelZoomModifier();

            var mGroup = new ModifierGroup();

            mGroup.ChildModifiers.Add(rbzm);
            mGroup.ChildModifiers.Add(mouseWheelZoomPanModifier);
            mGroup.ChildModifiers.Add(new ZoomExtentsModifier());
            scs.ChartModifier = mGroup;
        }
    }
}