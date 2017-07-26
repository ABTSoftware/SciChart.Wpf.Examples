// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Obj3D.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
namespace SciChart.Examples.ExternalDependencies.Common
{
    public class Obj3D : StrongTyped<string>
    {
        public Obj3D(string value) : base(value)
        {
        }

#if USE_HIGH_POLY_OBJ3D
        public static readonly Obj3D King = new Obj3D("King_High.obj");
        public static readonly Obj3D Queen = new Obj3D("Queen_High.obj");
        public static readonly Obj3D Bishop = new Obj3D("Bishop_High.obj");
        public static readonly Obj3D Knight = new Obj3D("Knight_High.obj");
        public static readonly Obj3D Rook = new Obj3D("Rook_High.obj");
        public static readonly Obj3D Pawn = new Obj3D("Pawn_High.obj");
#else
        public static readonly Obj3D King = new Obj3D("King_Low.obj");
        public static readonly Obj3D Queen = new Obj3D("Queen_Low.obj");
        public static readonly Obj3D Bishop = new Obj3D("Bishop_Low.obj");
        public static readonly Obj3D Knight = new Obj3D("Knight_Low.obj");
        public static readonly Obj3D Rook = new Obj3D("Rook_Low.obj");
        public static readonly Obj3D Pawn = new Obj3D("Pawn_Low.obj");
#endif
    }
}