using System.Diagnostics;
using System.Windows.Media.Media3D;
using SciChart.Charting3D;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Primitives;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Core.Utility.Mouse;

namespace Scatter3DChart_DragPointModifier
{
    public class TestWorldScreenCoordModifier3D : ChartModifierBase3D
    {
        public TestWorldScreenCoordModifier3D()
        {
            ReceiveHandledEvents = true;
        }

        private static Vector3D GetDataVector(Points3DSceneEntity points3DSceneEntity, EntityVertexId entityVertexId)
        {
            return points3DSceneEntity.GetVertexAt(new HitTestInfo3D
            {
                IsHit = true,
                EntityId = entityVertexId.EntityId,
                VertexId = entityVertexId.VertexId
            });
        }

        private static Vector3 GetWorldCoordVector(Points3DSceneEntity points3DSceneEntity, Vector3D dataVector)
        {
            return points3DSceneEntity.GetVertexCoords(dataVector);
        }

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);

            if (e.Modifier == MouseModifier.Shift)
            {
                var entityVertexId = Viewport3D.PickScene(e.MousePoint);

                if (entityVertexId.HasValue)
                {
                    Viewport3D.RootEntity.VisitEntities(entity =>
                    {
                        if (entity is Points3DSceneEntity points3DSceneEntity && entity.EntityId == entityVertexId.Value.EntityId)
                        {
                            points3DSceneEntity.PerformSelection(true, [new() { Id = entityVertexId.Value.VertexId }]);

                            var dataVector = GetDataVector(points3DSceneEntity, entityVertexId.Value);
                            var coordVectorWorld = GetWorldCoordVector(points3DSceneEntity, dataVector);

                            Debug.WriteLine($"RawMousePoint X: {e.MousePoint.X}, Y: {e.MousePoint.Y}");
                            Debug.WriteLine($"WorldVector   X: {coordVectorWorld.X}, Y: {coordVectorWorld.Y}, Z: {coordVectorWorld.Z} (HitTest)");

                            var screenPoint = Viewport3D.CameraController.WorldToScreenSpace(coordVectorWorld);
                         
                            Debug.WriteLine($"WorldToScreen X: {screenPoint.X}, Y: {screenPoint.Y}");

                            // What distance value should be used here and how to get it?
                            // Can ScreenToWorld calculate exactly the same world coordinates as the hit-test operation?
                            // The Vector From Camera.Position to MousePoint with Length of Distance
                            var worldVector = Viewport3D.CameraController.ScreenToWorldSpace(e.MousePoint, 1);
                            
                            Debug.WriteLine($"ScreenToWorld X: {worldVector.X}, Y: {worldVector.Y}, Z: {worldVector.Z}\n");
                        }
                    });
                }
            }
        }

        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            base.OnModifierMouseUp(e);

            Viewport3D.RootEntity.VisitEntities(entity => entity.PerformSelection(false, null));
        }
    }
}