// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// AntennaGainMeshEntity.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows.Media;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Primitives;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Antenna3DRadiationPattern
{
    /// <summary>
    /// Closed triangulated mesh for a 3-D antenna radiation pattern.
    ///
    /// Each (theta × phi) quad is split into two triangles.  All geometry,
    /// normals, and colours are precomputed once in the constructor because the
    /// gain data is static.  RenderScene only submits the cached triangles.
    ///
    /// Per-triangle normals are computed via the cross product of the two edge
    /// vectors.  The sign is resolved against the analytic outward direction in
    /// spherical coordinates — sin(θ)cos(φ), cos(θ), sin(θ)sin(φ) — so it
    /// stays robust near the equatorial gain-zero ring where vertices collapse
    /// toward the origin.
    /// </summary>
    public class AntennaGainMeshEntity : BaseSceneEntity<SCRTSceneEntity>
    {
        // Pre-transformed triangle with flat (per-face) normal and per-vertex colour.
        private struct CachedTriangle
        {
            public Vector3 Normal;
            public Color C0, C1, C2;
            public Vector3 V0, V1, V2;
        }

        private readonly CachedTriangle[] _triangles;

        /// <param name="gain">Normalised gain grid [theta, phi] in [0,1].</param>
        /// <param name="thetaCount">Number of elevation samples (0 to π inclusive).</param>
        /// <param name="phiCount">Number of azimuth samples (0 to 2π, last wraps to first).</param>
        /// <param name="scale">Data-space radius multiplier for the peak lobe.</param>
        /// <param name="palette">Colour palette mapping dBi values to vertex colours.</param>
        public AntennaGainMeshEntity(double[,] gain, int thetaCount, int phiCount,
                                     double scale, HeatmapColorPalette palette)
            : base(new SCRTSceneEntity())
        {
            _triangles = BuildTriangles(gain, thetaCount, phiCount, scale, palette,
                out double minX, out double maxX, out double minY, out double maxY,
                out double minZ, out double maxZ);
            MinX = minX; MaxX = maxX;
            MinY = minY; MaxY = maxY;
            MinZ = minZ; MaxZ = maxZ;
        }

        // Cartesian bounding box of the mesh, used to size axis VisibleRanges.
        public double MinX { get; }
        public double MaxX { get; }
        public double MinY { get; }
        public double MaxY { get; }
        public double MinZ { get; }
        public double MaxZ { get; }

        public override eSCRTSceneEntityKind GetKind() =>
            eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_OPAQUE;

        public override void RenderScene(IRenderPassInfo3D rpi)
        {
            using (var mesh = BeginLitMesh(TSRRenderMode.TRIANGLES))
            {
                VXccelEngine3D.PushRasterizerState(
                    RasterizerStates.Default.TSRRasterizerState);

                float xOff = rpi.WorldDimensions.X * 0.5f;
                float zOff = rpi.WorldDimensions.Z * 0.5f;

                foreach (var t in _triangles)
                {
                    // Transform data-space vertices through axis coordinate calculators
                    // so the mesh responds to VisibleRange changes on all three axes.
                    // X and Z need the world-center offset (GetCoordinate returns [0,WD],
                    // but the axis cube spans [-WD/2,+WD/2] for X and Z).
                    float wx0 = (float)rpi.XCalc.GetCoordinate(t.V0.X) - xOff;
                    float wy0 = (float)rpi.YCalc.GetCoordinate(t.V0.Y);
                    float wz0 = (float)rpi.ZCalc.GetCoordinate(t.V0.Z) - zOff;

                    float wx1 = (float)rpi.XCalc.GetCoordinate(t.V1.X) - xOff;
                    float wy1 = (float)rpi.YCalc.GetCoordinate(t.V1.Y);
                    float wz1 = (float)rpi.ZCalc.GetCoordinate(t.V1.Z) - zOff;

                    float wx2 = (float)rpi.XCalc.GetCoordinate(t.V2.X) - xOff;
                    float wy2 = (float)rpi.YCalc.GetCoordinate(t.V2.Y);
                    float wz2 = (float)rpi.ZCalc.GetCoordinate(t.V2.Z) - zOff;

                    mesh.Normal3(t.Normal.X, t.Normal.Y, t.Normal.Z);
                    mesh.SetVertexColor(t.C0); mesh.SetVertex3(wx0, wy0, wz0);
                    mesh.SetVertexColor(t.C1); mesh.SetVertex3(wx1, wy1, wz1);
                    mesh.SetVertexColor(t.C2); mesh.SetVertex3(wx2, wy2, wz2);
                }
            }

            VXccelEngine3D.PopRasterizerState();
        }

        private static CachedTriangle[] BuildTriangles(double[,] gain, int thetaCount,
                                                        int phiCount, double scale,
                                                        HeatmapColorPalette palette,
                                                        out double minX, out double maxX,
                                                        out double minY, out double maxY,
                                                        out double minZ, out double maxZ)
        {
            // Angular step sizes: θ spans [0,π], φ spans [0,2π) and wraps.
            double dTheta = Math.PI / (thetaCount - 1);
            double dPhi = 2.0 * Math.PI / phiCount;

            // Compute Cartesian bounding box over all mesh vertices.
            minX = double.MaxValue; maxX = double.MinValue;
            minY = double.MaxValue; maxY = double.MinValue;
            minZ = double.MaxValue; maxZ = double.MinValue;
            for (int ti = 0; ti < thetaCount; ti++)
            {
                double theta = ti * dTheta;
                double sinT = Math.Sin(theta), cosT = Math.Cos(theta);
                for (int pi = 0; pi < phiCount; pi++)
                {
                    double r = gain[ti, pi] * scale;
                    double x = r * sinT * Math.Cos(pi * dPhi);
                    double y = r * cosT;
                    double z = r * sinT * Math.Sin(pi * dPhi);
                    if (x < minX) minX = x; if (x > maxX) maxX = x;
                    if (y < minY) minY = y; if (y > maxY) maxY = y;
                    if (z < minZ) minZ = z; if (z > maxZ) maxZ = z;
                }
            }

            // Each (θ,φ) quad → 2 triangles; φ wraps so every row is a closed ring.
            var triangles = new CachedTriangle[(thetaCount - 1) * phiCount * 2];
            int idx = 0;

            for (int ti = 0; ti < thetaCount - 1; ti++)
            {
                for (int pi = 0; pi < phiCount; pi++)
                {
                    int pi1 = (pi + 1) % phiCount; // wrap azimuth

                    double theta0 = ti * dTheta;
                    double theta1 = (ti + 1) * dTheta;
                    double phi0 = pi * dPhi;
                    double phi1 = pi1 * dPhi;

                    // Four corners of this quad in Cartesian space.
                    Vector3 v00 = ToWorld(gain[ti,     pi ] * scale, theta0, phi0);
                    Vector3 v10 = ToWorld(gain[ti + 1, pi ] * scale, theta1, phi0);
                    Vector3 v01 = ToWorld(gain[ti,     pi1] * scale, theta0, phi1);
                    Vector3 v11 = ToWorld(gain[ti + 1, pi1] * scale, theta1, phi1);

                    // Map each vertex gain (normalised) → dBi → palette colour.
                    Color c00 = palette.GetColor(AntennaMetrics.ToDbi(gain[ti,     pi ])).ToColor();
                    Color c10 = palette.GetColor(AntennaMetrics.ToDbi(gain[ti + 1, pi ])).ToColor();
                    Color c01 = palette.GetColor(AntennaMetrics.ToDbi(gain[ti,     pi1])).ToColor();
                    Color c11 = palette.GetColor(AntennaMetrics.ToDbi(gain[ti + 1, pi1])).ToColor();

                    // Analytic outward direction at the quad centre — used to orient normals.
                    // Derived from spherical coords so it remains valid even when gain ≈ 0.
                    float refX = (float)(Math.Sin((theta0 + theta1) * 0.5) * Math.Cos((phi0 + phi1) * 0.5));
                    float refY = (float)(Math.Cos((theta0 + theta1) * 0.5));
                    float refZ = (float)(Math.Sin((theta0 + theta1) * 0.5) * Math.Sin((phi0 + phi1) * 0.5));

                    triangles[idx++] = new CachedTriangle
                    {
                        Normal = SurfaceNormal(v00, v01, v10, refX, refY, refZ),
                        C0 = c00, C1 = c01, C2 = c10,
                        V0 = v00, V1 = v01, V2 = v10
                    };
                    triangles[idx++] = new CachedTriangle
                    {
                        Normal = SurfaceNormal(v10, v01, v11, refX, refY, refZ),
                        C0 = c10, C1 = c01, C2 = c11,
                        V0 = v10, V1 = v01, V2 = v11
                    };
                }
            }

            return triangles;
        }

        /// <summary>
        /// Computes the unit normal of triangle (a, b, c) that is orthogonal to the
        /// mesh surface.  The sign is resolved by the caller-supplied analytic outward
        /// reference (refX, refY, refZ), which is the spherical-coordinate outward
        /// direction at the quad centre — always well-defined, never near-zero.
        /// Falls back to the reference vector itself for degenerate (zero-area) triangles.
        /// </summary>
        private static Vector3 SurfaceNormal(Vector3 a, Vector3 b, Vector3 c,
                                              float refX, float refY, float refZ)
        {
            float e1x = b.X - a.X, e1y = b.Y - a.Y, e1z = b.Z - a.Z;
            float e2x = c.X - a.X, e2y = c.Y - a.Y, e2z = c.Z - a.Z;

            float nx = e1y * e2z - e1z * e2y;
            float ny = e1z * e2x - e1x * e2z;
            float nz = e1x * e2y - e1y * e2x;

            float len = (float)Math.Sqrt(nx * nx + ny * ny + nz * nz);

            // Degenerate triangle (near-zero area) — use the analytic fallback.
            if (len < 1e-6f)
                return new Vector3(refX, refY, refZ);

            nx /= len; ny /= len; nz /= len;

            // Flip if the cross-product normal points inward relative to the
            // analytic outward direction.  This is always a reliable test because
            // (refX, refY, refZ) is derived from the angular coordinates, not from
            // the vertex positions, so it stays well-conditioned near gain = 0.
            if (nx * refX + ny * refY + nz * refZ < 0)
            { nx = -nx; ny = -ny; nz = -nz; }

            return new Vector3(nx, ny, nz);
        }

        // Spherical → Cartesian.  Convention: main beam along +Y (θ=0), φ rotates in XZ.
        private static Vector3 ToWorld(double r, double theta, double phi)
        {
            float x = (float)(r * Math.Sin(theta) * Math.Cos(phi));
            float y = (float)(r * Math.Cos(theta));
            float z = (float)(r * Math.Sin(theta) * Math.Sin(phi));
            return new Vector3(x, y, z);
        }

    }
}
