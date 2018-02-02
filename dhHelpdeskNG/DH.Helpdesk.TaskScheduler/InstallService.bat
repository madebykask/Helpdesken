set DefaultPathToInstallFile="DH.Helpdesk.TaskScheduler.exe"

set PathToInstallFile=%~dp0

if "%PathToInstallFile%"=="" set PathToInstallFile=%DefaultPathToInstallFile% 
if "%PathToInstallFile%"==" " set PathToInstallFile=%DefaultPathToInstallFile% 

"%windir%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "%PathToInstallFile%\%DefaultPathToInstallFile%"
pause