:: Shurtcut to run MSBuild.exe with GenerateAsseblyInfo.xml 
::	Run GenerateAssemblyInfo.bat [FILE_NAME] 
:: where FILE_NAME is where to put generated content for AssemblyInfo.cs
:: HINT: for more verbose MSBuild output add /v:d parameter
echo =========== resolving version from GIT ======================
%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe "%~dp0\GenerateAssemblyInfo.xml" /p:AssemblyFileName=%1
pause