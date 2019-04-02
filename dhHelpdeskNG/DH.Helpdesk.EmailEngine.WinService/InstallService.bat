set DefaultPathToInstallFile="%~dp0DH.Helpdesk.EmailEngine.WinService.exe"

set PathToInstallFile=%1 

if "%PathToInstallFile%"=="" set PathToInstallFile=%DefaultPathToInstallFile% 
if "%PathToInstallFile%"==" " set PathToInstallFile=%DefaultPathToInstallFile% 

"%windir%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "%PathToInstallFile%"
pause