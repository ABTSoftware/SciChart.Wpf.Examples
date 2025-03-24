param ([Parameter(Mandatory=$true)][string]$SourceDir)

Push-Location

Set-Location "$SourceDir\SharedLicensing\src\Abt.Licensing.Native"
.\Exec_swig.bat

Set-Location "$SourceDir\Core.cpp\src\Scichart.Charting.2D.Native"
.\Exec_swig.bat

Set-Location "$SourceDir\Core.cpp\src\Scichart.Charting.3D.Native"
.\Exec_swig.bat

Pop-Location