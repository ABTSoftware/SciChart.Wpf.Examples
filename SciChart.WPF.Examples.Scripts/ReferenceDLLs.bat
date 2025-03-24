@echo off

pushd tools
powershell -executionpolicy bypass -file ./ReferenceDLLs.ps1
popd

pause