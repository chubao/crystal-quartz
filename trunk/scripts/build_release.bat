echo off

FOR /F "TOKENS=3" %%A IN ('reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0" /v MSBuildToolsPath') DO SET MSBuildPath=%%Amsbuild

%MSBuildPath% "../src/CrystalQuartz.sln" /p:Configuration=Release


echo Building NuGet packages...

copy "..\bin\Release\CrystalQuartz.Core.dll" "..\nuget\simple\lib"
copy "..\bin\Release\CrystalQuartz.Web.dll" "..\nuget\simple\lib"
cd "..\nuget\simple"
nuget pack Package.nuspec -OutputDirectory "..\..\bin\NuGet"

pause