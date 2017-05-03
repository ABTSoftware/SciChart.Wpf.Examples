using System.Reflection;
using System.Windows.Controls;
using SciChart.Charting3D;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    /// <summary>
    /// Interaction logic for AddObjectsToA3DChart.xaml
    /// </summary>
    public partial class AddObjectsToA3DChart : UserControl
    {
        public AddObjectsToA3DChart()
        {
            InitializeComponent();

            CreateObjects();
        }

        private void CreateObjects()
        {
            // The model Bolt.obj is an embedded resource
            var objectData = DataManager.Instance.LoadWavefrontObject(Obj3D.Bolt);

            var obj = new ObjSceneEntity("Bolt", objectData);

            // We create an ObjSceneEntity and add to the scene 
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);

            //obj.Position = new Vector3(100,100,100);
        }
    }
}
