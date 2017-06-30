set DefaultPathToUninstallFile="%~dp0DH.Helpdesk.EmailEngine.WinService.exe"

set PathToUninstallFile=%1 

if "%PathToUninstallFile%"=="" set PathToUninstallFile=%DefaultPathToUninstallFile% 
if "%PathToUninstallFile%"==" " set PathToUninstallFile=%DefaultPathToUninstallFile% 

"%windir%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" /u "%PathToUninstallFile%"

pause