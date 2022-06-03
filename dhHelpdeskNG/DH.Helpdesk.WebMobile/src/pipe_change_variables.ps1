$fileNames = @(Get-ChildItem | Where-Object { $_.Name -match '^main\.[0-9a-zA-Z]+\.js$' } | Select-Object Name)
Foreach ($filename in $fileNames) {
	(Get-Content $fileName.Name).replace('API.URL', ${ApiUrl}) | Set-Content $fileName.Name
	(Get-Content $fileName.Name).replace('Microsoft.ShowLogin', ${MicrosoftShowLogin}) | Set-Content $fileName.Name
	(Get-Content $fileName.Name).replace('Microsoft.ClientId', 'ABC') | Set-Content $fileName.Name
	(Get-Content $fileName.Name).replace('Microsoft.Tenant', ${MicrosoftTenant}) | Set-Content $fileName.Name
	(Get-Content $fileName.Name).replace('Microsoft.Authority', ${MicrosoftAuthority}) | Set-Content $fileName.Name
	(Get-Content $fileName.Name).replace('Microsoft.RedirectUri', ${MicrosoftRedirectUri}) | Set-Content $fileName.Name
}