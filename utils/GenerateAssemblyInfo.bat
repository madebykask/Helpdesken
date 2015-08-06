:: Shurtcut to run MSBuild.exe with GenerateAsseblyInfo.xml 
::	Run GenerateAssemblyInfo.bat [FILE_NAME] 
:: where FILE_NAME is where to put generated content for AssemblyInfo.cs
echo =========== resolving version from GIT ======================
%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe /toolsversion:4.0 %~dp0\GenerateAssemblyInfo.xml /p:AssemblyFileName=%1