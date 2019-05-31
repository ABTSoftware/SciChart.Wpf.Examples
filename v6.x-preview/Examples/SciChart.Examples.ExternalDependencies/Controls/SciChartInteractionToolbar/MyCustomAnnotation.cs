using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SciChart.Charting.Visuals.Annotations;

namespace SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar
{
    public class MyCustomAnnotation : CustomAnnotation
    {
       public MyCustomAnnotation()
       {

           var uri = new Uri(@"C:\Projects\SciChart\branches\SciChart_v4.2_Release\src\Examples\SciChart.Examples.ExternalDependencies\Resources\ImagesSciChartLogo_Dark.png");
           
           BitmapImage bimage = new BitmapImage();
           bimage.BeginInit();
           bimage.UriSource = uri;
           bimage.EndInit();
           var image =  new Image { Source = bimage };
           image.Width = 178;
           image.Height = 74;
           image.Stretch = Stretch.None;
           Content = image;
       }

    }
}
