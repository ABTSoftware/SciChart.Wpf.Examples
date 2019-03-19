# DirectXDeploymentTestApp 

These applications have been created to show users of SciChart best practices how to create and deploy applications with the DirectX renderer plugin. 

The DirectX Renderer plugin is not enabled by default in SciChart, and has a dependency on SharpDX. There is also a dependency on a C++ library shipped by SharpDX called sharpdx_direct3d11_1_effects_x64.dll which must be included if you are not referencing SharpDX from NuGet. It is this library which causes some problems for us at SciChart, but can easily be worked around. 

See related resources: 

 * [SciChart website: Creating and deploying applications with DirectX](https://support.scichart.com/index.php?/Knowledgebase/Article/View/17262/44/creating-and-deploying-applications-with-the-direct3d11rendersurface)
 * [SciChart Forums: DLLNotFoundException Problem with SharpDX in Designer in visual studio](https://www.scichart.com/questions/question/dllnotfoundexception-sharpdx_direct3d11_1_effects_x86-dll)
 * [SharpDX on Github: sharpdx_direct3d11_1_effects_x64.dll not included in project from NuGet](https://github.com/sharpdx/SharpDX/issues/941)
 * [Stackoverflow: How to include sharpdx_direct3d11_1_effects_x64.dll in your project](https://stackoverflow.com/questions/46549637/sharpdx-v4-0-1-sharpdx-direct3d11-effects-x64-dll-dllnotfoundexception)
 
### NuGet (Package Reference version)

This version of the project references [SciChart.DirectX](https://www.nuget.org/packages/SciChart.DirectX/) and [SciChart](https://www.nuget.org/packages/SciChart/) Nuget packages using Visual Studio PackageReference. 


### NuGet (Packages.config version)

This version of the project references [SciChart.DirectX](https://www.nuget.org/packages/SciChart.DirectX/) and [SciChart](https://www.nuget.org/packages/SciChart/) Nuget packages using Visual Studio Packages.config.

In both the NuGet Packages.config and PackageReference version, sharpdx_direct3d11_1_effects_x64.dll and sharpdx_direct3d11_1_effects_x86.dll are included automatically by the SharpDX Nuget packages. 

**NOTE:** If you reference SharpDX without NuGet you will need to include both sharpdx_direct3d11_1_effects_x64.dll and sharpdx_direct3d11_1_effects_x86.dll yourself. These are included in the SciChart Installer download.

# Using the Application 

Just build and run it. Notice the Annotation in the middle of the chart which shows you which renderer plugin is applied.

![Designer View](https://github.com/ABTSoftware/SciChart.WPF.Examples/raw/master/v5.x/Sandbox/DirectXDeploymentTestApp/Runtime%20View%20showing%20DirectX%20Renderer.png "Designer View")

![Runtime View](https://github.com/ABTSoftware/SciChart.WPF.Examples/raw/master/v5.x/Sandbox/DirectXDeploymentTestApp/Runtime%20View%20showing%20DirectX%20Renderer.png "Runtime View")

