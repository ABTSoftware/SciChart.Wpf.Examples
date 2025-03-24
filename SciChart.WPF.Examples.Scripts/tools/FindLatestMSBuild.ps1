$msbuild = & "${env:ProgramFiles(x86)}/Microsoft Visual Studio/Installer/vswhere.exe" -products * -latest -requires Microsoft.Component.MSBuild -find MSBuild/**/Bin/MSBuild.exe | select-object -first 1
if (-not (test-path $msbuild)) {
    Write-Host "Unable to find MSBuild"           
    return -1
}
return $msbuild 