param (
    [string]$PackageVersion = $(Read-Host "NuGet package version"), #7.0.1.27097
    [string]$ExamplesDir = $(Read-Host "SciChart.Examples root directory") #C:\Workspace\SciChart.Examples
)

if (-not ($PackageVersion -match "^[0-9]\d*(\.[0-9]\d*)*$")) {
    Write-Host Invalid NuGet package version string format
    exit -1
}

if (-not (Test-Path $ExamplesDir)) {
    Write-Host SciChart.Examples directory does not exist
    exit -1
}

foreach ($dir in (Get-ChildItem -Path $ExamplesDir -Filter "NuGet.config" -File -Recurse | %{$_.DirectoryName})) {

    Copy-Item ".\data\NuGet.config" -Destination $dir -Force
    Write-Host Directory: $dir',' updated NuGet.config file
}

$packageNames = "SciChart",
                "SciChart3D",
                "SciChart.DrawingTools",
                "SciChart.ExternalDependencies"

.\AddNuGetReferences.ps1 $ExamplesDir $PackageVersion $packageNames