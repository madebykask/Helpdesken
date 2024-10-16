#################################################################################################
#                                                                                               #
# Searches for user in Azure with help of ms-graph,                                             #
# using appregistration at Älmhult                                        #
#                                                                                               #
# Version history:                                                                              #
# 2020-12-08, Björn Lebell, Example to fetch users with microsoft Graph                         #
# 2022-08-22, Jessica Södergren, Added Tls1.2                                                   #
#                                                                                               #
#################################################################################################



# static ones grabbed from azure portal -> app registration for authentication to Graph (here it is token based…)
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12
$AppId = "a07dba75-7a08-4a67-a349-68dc732c6db0"
$TenantName = "kommunsamverkan.onmicrosoft.com"
$Scope = "https://graph.microsoft.com/.default"
$AppSecret = "tAn8Q~jSCQjVlGDIdZiQ5dgOjgy2OhZyAnD_jdvE"
$redirectUri = "https://localhost/graph"
$Url = "https://login.microsoftonline.com/$TenantName/oauth2/v2.0/token"



# retrieve the token in Microsoft standard way 
Add-Type -AssemblyName System.Web

$Body = @{
    client_id = $AppId
            client_secret = $AppSecret
            scope = $Scope
            grant_type = 'client_credentials'
}

$PostSplat = @{
    ContentType = 'application/x-www-form-urlencoded'
    Method = 'POST'
    Body = $Body
    Uri = $Url
}

$Request = Invoke-RestMethod @PostSplat

$Header = @{
    Authorization = "$($Request.token_type) $($Request.access_token)"
    #Needed to be able to use endsWith filter
    ConsistencyLevel = "eventual"
}
# Microsoft standard call ends

#Exectution and storing…
$Result = Invoke-RestMethod `
        -Uri "https://graph.microsoft.com/v1.0/users?`$select=displayName,givenName,jobTitle,mail,mobilePhone,officeLocation,preferredLanguage,surname,companyName,country,userPrincipalName,accountEnabled,city,creationType,department,employeeId,id,mailNickname,userType,extension_5aa67923d5cf4c8897970c325506ca08_employeeNumber&`$filter=(endswith(mail,'almhult.se') OR endswith(mail,'almhultsbostader.se')) AND AccountEnabled eq true&`$count=true" `
        -Headers $Header
$Users = $Result.value
while ($Result.'@odata.nextLink') {
    Write-Host "Getting another page of 100 users..."
	Write-Host $Result.'@odata.nextLink'
    $Result = Invoke-RestMethod -Uri $Result.'@odata.nextLink' -Headers $Header
    $Users += $Result.value
}

#process what’s been found...
$Users | Export-CSV -Path D:\Websites\services\CSVImportAlmhultIT\temp.csv -Delimiter ";" -NoTypeInformation -Encoding unicode


#Replace all " with empty
$Temp = Get-Content D:\Websites\services\CSVImportAlmhultIT\temp.csv
$Temp.Replace('"','') | Out-File D:\Websites\services\CSVImportAlmhultIT\temp2.csv -Force -Confirm:$false
Remove-Item D:\Websites\services\CSVImportAlmhultIT\temp.csv

#Replace 
$Temp2 = Get-Content D:\Websites\services\CSVImportAlmhultIT\temp2.csv
$Temp2.Replace('extension_5aa67923d5cf4c8897970c325506ca08_employeeNumber', 'Personnummer') | Out-File D:\Websites\services\CSVImportAlmhultIT\initiators.csv -Force -Confirm:$false
Remove-Item D:\Websites\services\CSVImportAlmhultIT\temp2.csv