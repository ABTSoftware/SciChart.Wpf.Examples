param (
    [string]$SourceDir = $(Read-Host "SciChart source root directory"), #C:\Workspace\SciChart.Release70
    [string]$ExamplesDir = $(Read-Host "SciChart.Examples root directory") #C:\Workspace\SciChart.Examples
)

if (!$SourceDir -or !(Test-Path $SourceDir)) {
    Write-Host Source code directory does not exist
    exit -1
}

if (!$ExamplesDir -or !(Test-Path $ExamplesDir)) {
    Write-Host SciChart.Examples directory does not exist
    exit -1
}

$buildSln = $false;

do {
    $response = Read-Host -Prompt 'Do you want to rebuild SciChart.Dev solution? [Y/N]'

    if ($response -eq 'y') { $buildSln = $true }

} until ($response -eq 'y' -or $response -eq 'n')


if ($buildSln) {

    $slnDir = "$SourceDir\WPF\src"

    . .\SafeRemoveDir.ps1

    SafeRemoveDir("$slnDir\Build")
    
    $projectsToClean = "SciChart.Charting",
                       "SciChart.Charting.3D",
                       "SciChart.Core",
                       "SciChart.Data",
                       "SciChart.Drawing",
                       "SciChart.Drawing.DirectX",
					   "Examples\SciChart.Examples.ExternalDependencies"

    $projectsToClean | ForEach-Object {"$slnDir\$_\bin"} | SafeRemoveDir
    $projectsToClean | ForEach-Object {"$slnDir\$_\obj"} | SafeRemoveDir
    
    $msbuild = & .\FindLatestMSBuild.ps1
    if (-not (Test-Path $msbuild)) {
        Write-Host "Error finding MSBuild"
        exit -1
    }
    
    .\SwigProjects.ps1 $SourceDir
    
    $restoreMsBuildArgs = @("$slnDir\SciChart.Dev.sln", 
        "-target:Clean;Restore", 
        "-verbosity:normal")               
    & $msbuild $restoreMsBuildArgs
    
    if ($LastExitCode -ne 0) {
        Write-Host Error building solution',' code $LastExitCode
        exit $LastExitCode
    }
    
    $buildMsBuildArgs = @("$slnDir\SciChart.Dev.sln", 
        "-target:Build", 
        "-toolsVersion:Current", 
        "-p:Configuration=Release;Platform=`"Any CPU`";WarningLevel=0", 
        "-m", 
        "-verbosity:normal")
    & $msbuild $buildMsBuildArgs
    
    if ($LastExitCode -ne 0) {
        Write-Host Error building solution',' code $LastExitCode
        exit $LastExitCode
    }
}

$dllsToReference = "SciChart.Charting",
                   "SciChart.Charting3D",
                   "SciChart.Core",
                   "SciChart.Data",
                   "SciChart.Drawing",
				   "SciChart.Examples.ExternalDependencies"

$dllsDir = "$SourceDir\WPF\src\Build\Release"

if (-not (($dllsToReference | ForEach-Object {"$dllsDir\net462\$_.dll"} | Test-Path) -notcontains $false)) {
    Write-Host 'SciChart.*.dll files could not be found for net462'
    exit -1
}

if (-not (($dllsToReference | ForEach-Object {"$dllsDir\net6.0-windows\$_.dll"} | Test-Path) -notcontains $false)) {
    Write-Host 'SciChart.*.dll files could not be found for net6.0-windows'
    exit -1
}

if (-not (($dllsToReference | ForEach-Object {"$dllsDir\netcoreapp3.1\$_.dll"} | Test-Path) -notcontains $false)) {
    Write-Host 'SciChart.*.dll files could not be found for netcoreapp3.1'
    exit -1
}

.\AddLocalReferences.ps1 $ExamplesDir $dllsDir $dllsToReference