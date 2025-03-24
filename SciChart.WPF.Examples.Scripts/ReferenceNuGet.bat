@echo off

pushd tools
powershell -executionpolicy bypass -file ./ReferenceNuGet.ps1
popd

pause