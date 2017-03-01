using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SciChart.Examples.Demo.Controls.Breadcrumb
{    
    public class BreadcrumbControl : ContentControl
    {
        public static readonly DependencyProperty HomeCommandProperty = DependencyProperty.Register("HomeCommand", typeof (ICommand), typeof (BreadcrumbControl), new PropertyMetadata(default(ICommand)));
        public static readonly DependencyProperty BreadcrumbItemsProperty = DependencyProperty.Register("BreadcrumbItems", typeof (IEnumerable), typeof (BreadcrumbControl), new PropertyMetadata(default(IEnumerable)));

        public static readonly DependencyProperty HomeButtonStyleProperty = DependencyProperty.Register(
            "HomeButtonStyle", typeof(Style), typeof(BreadcrumbControl), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty BreadcrumbItemStyleProperty = DependencyProperty.Register(
            "BreadcrumbItemStyle", typeof(Style), typeof(BreadcrumbControl), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty SeparatorTemplateProperty = DependencyProperty.Register(
            "SeparatorTemplate", typeof (DataTemplate), typeof (BreadcrumbControl), new PropertyMetadata(default(DataTemplate)));

        public BreadcrumbControl()
        {
            DefaultStyleKey = typeof (BreadcrumbControl);
        }

        public ICommand HomeCommand
        {
            get { return (ICommand)GetValue(HomeCommandProperty); }
            set { SetValue(HomeCommandProperty, value); }
        }

        public IEnumerable BreadcrumbItems
        {
            get { return (IEnumerable)GetValue(BreadcrumbItemsProperty); }
            set { SetValue(BreadcrumbItemsProperty, value); }
        }

        public Style HomeButtonStyle
        {
            get { return (Style)GetValue(HomeButtonStyleProperty); }
            set { SetValue(HomeButtonStyleProperty, value); }
        }

        public Style BreadcrumbItemStyle
        {
            get { return (Style)GetValue(BreadcrumbItemStyleProperty); }
            set { SetValue(BreadcrumbItemStyleProperty, value); }
        }

        public DataTemplate SeparatorTemplate
        {
            get { return (DataTemplate)GetValue(SeparatorTemplateProperty); }
            set { SetValue(SeparatorTemplateProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            int i = 0;
        }
    }
}
