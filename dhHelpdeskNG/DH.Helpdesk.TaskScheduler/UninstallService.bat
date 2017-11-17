set DefaultPathToUninstallFile="DH.Helpdesk.TaskScheduler.exe"

set PathToUninstallFile=%~dp0

if "%PathToUninstallFile%"=="" set PathToUninstallFile=%DefaultPathToUninstallFile% 
if "%PathToUninstallFile%"==" " set PathToUninstallFile=%DefaultPathToUninstallFile% 

"%windir%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" /u "%PathToUninstallFile%\%DefaultPathToUninstallFile%"
pause