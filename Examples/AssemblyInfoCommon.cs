// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AssemblyInfoCommon.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Reflection;
using System.Runtime.InteropServices;

// WPF: By default, we dont encrypt methods, strings, constants. These are turned on selectively for sensitive code
//[assembly: Obfuscation(Feature = "msil encryption, string encryption, value encryption", Exclude = true, StripAfterObfuscation = true)]

[assembly: AssemblyCompany("SciChart Ltd")]
[assembly: AssemblyCopyright("Copyright © SciChart Ltd 2011-2021, www.scichart.com")]
[assembly: AssemblyTrademark("SCICHART™")]
 
//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("PublicPrivateKeyFile.snk")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

// NOTE: MASTER SHOULD KEEP THIS AS V7.x WHEN MERGING. 
// This is to prevent conflicts in teamcity when tests are run on both master & v6 branch at the same time. 
[assembly: AssemblyVersion("6.4.0.13674")]
