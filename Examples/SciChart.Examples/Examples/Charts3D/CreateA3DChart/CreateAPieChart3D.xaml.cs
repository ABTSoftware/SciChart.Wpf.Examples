using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.RenderableSeries;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    /// <summary>
    /// Interaction logic for CreateAPieChart.xaml
    /// </summary>
    public partial class CreateAPieChart : UserControl
    {
        public CreateAPieChart()
        {
            InitializeComponent();

            var sceneEntity = PieSeries3D.GetSceneEntity() as SciChart.Charting3D.RenderableSeries.PieMeshSceneEntity;

            // Pie
            PieSection pieSection = new PieSection();
            pieSection.fInnerRadius = 40.0f;
            pieSection.fOuterRadius = 160.0f;
            pieSection.fPercentage = 0.2f;
            pieSection.fStartAngle = 0.0f;
            pieSection.fThickness = 30.0f;
            pieSection.color = System.Windows.Media.Color.FromArgb(255, 0, 0, 255);
            sceneEntity.AddSection(pieSection);

            pieSection.fInnerRadius = 40.0f;
            pieSection.fOuterRadius = 140.0f;
            pieSection.fPercentage = 0.2f;
            pieSection.fStartAngle = 0.2f * ( float )Math.PI * 2.0f;
            pieSection.fThickness = 40.0f;
            pieSection.color = System.Windows.Media.Color.FromArgb(255, 0, 255, 0);
            sceneEntity.AddSection(pieSection);

            pieSection.fInnerRadius = 60.0f;
            pieSection.fOuterRadius = 120.0f;
            pieSection.fPercentage = 0.2f;
            pieSection.fStartAngle = 0.4f * (float)Math.PI * 2.0f;
            pieSection.fThickness = 50.0f;
            pieSection.color = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);
            sceneEntity.AddSection(pieSection);

            pieSection.fInnerRadius = 60.0f;
            pieSection.fOuterRadius = 120.0f;
            pieSection.fPercentage = 0.2f;
            pieSection.fStartAngle = 0.6f * (float)Math.PI * 2.0f;
            pieSection.fThickness = 50.0f;
            pieSection.color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
            sceneEntity.AddSection(pieSection);

            pieSection.fInnerRadius = 60.0f;
            pieSection.fOuterRadius = 120.0f;
            pieSection.fPercentage = 0.2f;
            pieSection.fStartAngle = 0.8f * (float)Math.PI * 2.0f;
            pieSection.fThickness = 50.0f;
            pieSection.color = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);

            sceneEntity.AddSection(pieSection);


            diffuseColorComboBox.ItemsSource = typeof(Colors).GetProperties().Select(x => new ColorModel { ColorName = x.Name, Color = (Color)x.GetValue(null, null) }).ToList();
            specularColorComboBox.ItemsSource = typeof(Colors).GetProperties().Select(x => new ColorModel { ColorName = x.Name, Color = (Color)x.GetValue(null, null) }).ToList();
        }

        private void DiffuseColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PieSeries3D.DiffuseColor = ((ColorModel)diffuseColorComboBox.SelectedItem).Color;
        }

        private void SpecularColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PieSeries3D.SpecularColor = ((ColorModel)specularColorComboBox.SelectedItem).Color;
        }
    }

    public class ColorModel
    {
        public Color Color { get; set; }

        public Brush Brush
        {
            get { return new SolidColorBrush(Color); }
        }

        public string ColorName { get; set; }
    }
}
