using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Visuals.Object;
using SciChart.Core.Extensions;
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
            var dataSeries = new UniformGridDataSeries3D<double>(9, 9) {StartX = 1, StepX = 1, StartZ = 100, StepZ = 1};

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (y%2 == 0)
                    {
                        dataSeries[y, x] = (x%2 == 0 ? 1 : 4);
                    }
                    else
                    {
                        dataSeries[y, x] = (x%2 == 0 ? 4 : 1);
                    }
                }
            }

            surfaceMeshRenderableSeries.DataSeries = dataSeries;
        }
    }

    public class Object3DSource : IObj3DSource
    {
        public string _sourcePath;

        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                _sourcePath = value;
                if (value != null)
                {
                    Obj3DBytesSource = LoadWavefrontObject(value);
                }
            }
        }

        public byte[] Obj3DBytesSource { get; private set; }

        /// <summary>
        /// This loads an *.obj file which has been embedded in the SciChart.Examples.ExternalDependencies as an 
        /// embedded resource. Valid obj files include the values provided by <see cref="Obj3D"/> type
        /// </summary>
        /// <param name="objResource">The resource to load</param>
        /// <returns>A byte[] array representing the obj file</returns>
        public byte[] LoadWavefrontObject(string objResource)
        {
            byte[] result = null;
            if (!objResource.IsNullOrEmpty())
            {
                result = DataManager.Instance.LoadWavefrontObject(new Obj3D(objResource));
            }

            return result;
        }

        /// <summary>
        /// This loads an *.obj file which has been embedded in the SciChart.Examples.ExternalDependencies as an 
        /// embedded resource. Valid obj files include the values provided by <see cref="Obj3D"/> type
        /// </summary>
        /// <param name="objResource">The resource to load</param>
        /// <returns>A byte[] array representing the obj file</returns>
        public byte[] LoadWavefrontObjectFromPath(string obj3DSource)
        {
            byte[] result = null;
            if (File.Exists(obj3DSource))
            {
                using (var stream = new FileStream(obj3DSource, FileMode.Open))
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    result = ms.ToArray();
                }
            }

            return result;
        }
    }
}
