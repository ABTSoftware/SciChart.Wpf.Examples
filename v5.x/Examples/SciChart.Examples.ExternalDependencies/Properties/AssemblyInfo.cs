// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AssemblyInfo.cs is part of SCICHART®, High Performance Scientific Charts
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

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("SciChart.Examples.ExternalDependencies")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("SciChart.Examples.ExternalDependencies")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("216b5db5-89a3-44e0-85fd-2d9c6535e27d")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the AppPage, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the AppPage, 
    // app, or any theme specific resource dictionaries)
)]

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
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Common")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Behaviors")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Data")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Helpers")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar.Extension")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar.CustomModifiers")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.SciChartInteractionToolbar.Converters")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.SciChart3DInteractionToolbar")]

[assembly: XmlnsPrefix("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "ext")]
