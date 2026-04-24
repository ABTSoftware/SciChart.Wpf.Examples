// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ComboBoxItemTemplateSelector.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D
{
    public class ComboBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SelectedItemTemplate { get; set; }

        public DataTemplate ItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var isSelected = false;

            var fe = container as FrameworkElement;
            if (fe != null)
            {
                var parent = fe.TemplatedParent;
                if (parent != null)
                {
                    var cbo = parent as ComboBox;

                    isSelected = cbo != null;
                }
            }

            return isSelected ? SelectedItemTemplate : ItemTemplate;
        }
    }
}
