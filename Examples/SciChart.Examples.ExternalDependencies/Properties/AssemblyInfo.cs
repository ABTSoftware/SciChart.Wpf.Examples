// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// AssemblyInfo.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

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

[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Common")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Behaviors")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Data")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Helpers")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.Toolbar2D")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.Extension")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.Converters")]
[assembly: XmlnsDefinition("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "SciChart.Examples.ExternalDependencies.Controls.SciChart3DInteractionToolbar")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/expression/2010/interactivity", "Microsoft.Xaml.Behaviors")]

[assembly: XmlnsPrefix("http://schemas.abtsoftware.co.uk/scichart/exampleExternals", "ext")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/expression/2010/interactivity", "i")]