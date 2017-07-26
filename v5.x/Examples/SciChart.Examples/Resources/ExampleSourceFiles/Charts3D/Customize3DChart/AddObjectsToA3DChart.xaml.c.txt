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
            const int BoardOriginOffset = -100;
            const int PieceSize = 20;
            const int PieceHalpSize = PieceSize / 2;

            byte[] objectData;
            ObjSceneEntity obj;

            // a2 - h2: White Pawns
            objectData = DataManager.Instance.LoadWavefrontObject(Obj3D.Pawn);
            for (int i = 0; i < 8; i++)
            {
                obj = new ObjSceneEntity(Obj3D.Pawn.Value, objectData);
                sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
                obj.Position = new Vector3(BoardOriginOffset + PieceSize * i + PieceHalpSize, 0, BoardOriginOffset + PieceSize + PieceHalpSize);
                obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            
            // a2 - h2: Black Pawns
            for (int i = 0; i < 8; i++)
            {
                objectData = DataManager.Instance.LoadWavefrontObject(Obj3D.Pawn);
                obj = new ObjSceneEntity(Obj3D.Pawn.Value, objectData);
                sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
                obj.Position = new Vector3(BoardOriginOffset + PieceSize * i + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 6 + PieceHalpSize);
                obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            // a1: White Left Rook
            objectData = DataManager.Instance.LoadWavefrontObject(Obj3D.Rook);
            obj = new ObjSceneEntity(Obj3D.Rook.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceHalpSize, 0, BoardOriginOffset + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // h1: White Right Rook
            obj = new ObjSceneEntity(Obj3D.Rook.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 7 + PieceHalpSize, 0, BoardOriginOffset + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // a8: Black Left Rook
            obj = new ObjSceneEntity(Obj3D.Rook.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 7 + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // h8: Black Right Rook
            obj = new ObjSceneEntity(Obj3D.Rook.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 7 + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 7 + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // b1: White Left Knight
            objectData = DataManager.Instance.LoadWavefrontObject(Obj3D.Knight);
            obj = new ObjSceneEntity(Obj3D.Knight.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize + PieceHalpSize, 0, BoardOriginOffset + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // g1: White Right Knight
            obj = new ObjSceneEntity(Obj3D.Knight.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 6 + PieceHalpSize, 0, BoardOriginOffset + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // b8: Black Left Knight
            obj = new ObjSceneEntity(Obj3D.Knight.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 7 + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // g8: Black Right Knight
            obj = new ObjSceneEntity(Obj3D.Knight.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 6 + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 7 + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // c1: White Left Bishop
            objectData = DataManager.Instance.LoadWavefrontObject(Obj3D.Bishop);
            obj = new ObjSceneEntity(Obj3D.Bishop.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 2 + PieceHalpSize, 0, BoardOriginOffset + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // f1: White Right Bishop
            obj = new ObjSceneEntity(Obj3D.Bishop.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 5 + PieceHalpSize, 0, BoardOriginOffset + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // c8: Black Left Bishop
            obj = new ObjSceneEntity(Obj3D.Bishop.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 2 + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 7 + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // f8: Black Right Bishop
            obj = new ObjSceneEntity(Obj3D.Bishop.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 5 + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 7 + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // d1: White Queen
            objectData = DataManager.Instance.LoadWavefrontObject(Obj3D.Queen);
            obj = new ObjSceneEntity(Obj3D.Queen.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 3 + PieceHalpSize, 0, BoardOriginOffset + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // d8: Black Queen
            obj = new ObjSceneEntity(Obj3D.Queen.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 3 + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 7 + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // d1: White King
            objectData = DataManager.Instance.LoadWavefrontObject(Obj3D.King);
            obj = new ObjSceneEntity(Obj3D.King.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 4 + PieceHalpSize, 0, BoardOriginOffset + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);

            // d8: Black King
            obj = new ObjSceneEntity(Obj3D.King.Value, objectData);
            sciChart3DSurface.Viewport3D.RootEntity.Children.Add(obj);
            obj.Position = new Vector3(BoardOriginOffset + PieceSize * 4 + PieceHalpSize, 0, BoardOriginOffset + PieceSize * 7 + PieceHalpSize);
            obj.Scale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
