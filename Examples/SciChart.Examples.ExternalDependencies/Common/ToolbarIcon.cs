using System.Windows;
using MahApps.Metro.IconPacks;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class ToolbarIcon : PackIconMaterial
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register
            (nameof(Path), typeof(string), typeof(ToolbarIcon), new PropertyMetadata(null, OnPathPropertyChanged));

        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        protected override void UpdateData()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                Data = Path;
            }
            else if (Kind != 0)
            {
                string value = null;
                PackIconMaterialDataFactory.DataIndex.Value?.TryGetValue(Kind, out value);
                Data = value;
            }
            else
            {
                Data = null;
            }
        }

        private static void OnPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ToolbarIcon toolbarIcon)
            {
                toolbarIcon.UpdateData();
            }
        }
    }
}