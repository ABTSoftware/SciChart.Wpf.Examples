using System;
using System.Windows.Input;
using SciChart.Charting3D;
using SciChart.Charting3D.Modifiers;

namespace KeyboardMoveXozModifier3DExample
{
    public class KeyboardMoveXozModifier3D : FreeLookModifier3D
    {
        private static readonly Vector3 _unitY = new Vector3(0, 1, 0);

        /// <summary>
        /// Computes position, target vectors and ortho zoom factor depending on keys pressed
        /// </summary>
        /// <param name="camera">The camera.</param>
        /// <param name="positionVector">The position vector.</param>
        /// <param name="targetVector">The target vector.</param>
        /// <param name="orthoZoomFactor">The ortho zoom factor.</param>
        /// <returns>True if any update occurred</returns>
        protected override bool GetVectors(ICameraController camera, out Vector3 positionVector, out Vector3 targetVector, out float orthoZoomFactor)
        {
            bool anyPressed = false;
            positionVector = Vector3.Zero;
            targetVector = Vector3.Zero;
            orthoZoomFactor = 1.0f;

            if (Keyboard.IsKeyDown(ForwardKey))
            {
                using (var fwd = GetForwardVector(camera))
                {
                    targetVector += fwd;
                    positionVector += fwd;
                }
                orthoZoomFactor += -2.0f * camera.Forward.Length / camera.Radius;
                anyPressed = true;
            }
            if (Keyboard.IsKeyDown(BackKey))
            {
                using (var fwd = GetForwardVector(camera))
                {
                    targetVector -= fwd;
                    positionVector -= fwd;
                }
                orthoZoomFactor += 2.0f * camera.Forward.Length / camera.Radius;
                anyPressed = true;
            }

            if (Keyboard.IsKeyDown(RightKey))
            {
                positionVector += camera.Side;
                targetVector += camera.Side;
                anyPressed = true;
            }
            if (Keyboard.IsKeyDown(LeftKey))
            {
                positionVector -= camera.Side;
                targetVector -= camera.Side;
                anyPressed = true;
            }

            return anyPressed;
        }

        private static Vector3 GetForwardVector(ICameraController camera)
        {
            var fwd = camera.Forward;
            var fwdDotUnitY = Vector3.DotProduct(fwd, _unitY);
            if (Math.Abs(fwdDotUnitY) > 0.9)
            {
                fwd = camera.Up;
            }

            if (fwdDotUnitY > 0.0)
            {
                fwd.Negate();
            }

            fwd.Y = 0;
            fwd.Normalize();

            return fwd;
        }
    }
}
