# Inställningar för SQL Server
$server = "135.225.90.241"
$database = "DH_Helpdesk"
$username = "DH_Helpdesk"
$password = "arita860"
$query = @"
SELECT EwsClientSecretExpireDate, Customer_Id, b.Name 
FROM tblSettings 
INNER JOIN tblCustomer b ON b.Id = tblSettings.Customer_Id 
WHERE EwsClientSecretExpireDate <= GETDATE() + 30
"@

# Inställningar för Graph API
$tenantId = "4a1e41ed-d531-4001-9d5d-680f3587abb6"
$clientId = "c83d7b9e-4725-4c9d-a407-e556ecd027f6"
$clientSecret = "V4u8Q~PUuHGi7a89qpb.3wwlSqnjQ~L6qKNNZdn4"
$scope = "https://graph.microsoft.com/.default"
$mailTo = "camilla.lindvall@dhsolutions.se"
$mailFrom = "support@dhsolutions.se"

# Funktion för att hämta åtkomsttoken från Microsoft Graph
function Get-GraphToken {
    $body = @{
        grant_type    = "client_credentials"
        client_id     = $clientId
        client_secret = $clientSecret
        scope         = $scope
        tenant        = $tenantId
    }

    $response = Invoke-RestMethod -Method Post -Uri "https://login.microsoftonline.com/$tenantId/oauth2/v2.0/token" -Body $body
    return $response.access_token
}

# Function to send email via Graph API with UTF-8 encoding
function Send-GraphEmail($token, $recipient, $subject, $body) {
    $email = @{
        message = @{
            subject = $subject
            body    = @{
                contentType = "Text"
                content     = $body
            }
            toRecipients = @(@{ emailAddress = @{ address = $recipient } })
        }
    } | ConvertTo-Json -Depth 10

    # Convert to UTF-8 to avoid character issues
    $emailUTF8 = [System.Text.Encoding]::UTF8.GetBytes($email)

    Invoke-RestMethod -Uri "https://graph.microsoft.com/v1.0/users/$mailFrom/sendMail" `
                      -Method Post `
                      -Headers @{ Authorization = "Bearer $token"; "Content-Type" = "application/json; charset=UTF-8" } `
                      -Body $emailUTF8
}

# Secure database connection
$connectionString = "Server=$server;Database=$database;User Id=$username;Password=$password;"
$connection = New-Object System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$connection.Open()

$command = $connection.CreateCommand()
$command.CommandText = $query
$reader = $command.ExecuteReader()

$rows = @()
while ($reader.Read()) {
    $datum = $reader.GetDateTime(0)
    $customerId = $reader.GetInt32(1)  # Assuming Customer_Id is an INT
    $customerName = $reader.GetString(2)  # Assuming Name is a string

    # Format date in Swedish
    $swedishDate = $datum.ToString("yyyy-MM-dd", [System.Globalization.CultureInfo]::GetCultureInfo("sv-SE"))

    $rows += [PSCustomObject]@{
        ExpireDate   = $swedishDate
        CustomerId   = $customerId
        CustomerName = $customerName
    }
}

$reader.Close()
$connection.Close()

# Send emails if any expiration date is within 30 days
if ($rows.Count -gt 0) {
    $token = Get-GraphToken

    foreach ($row in $rows) {
        $subject = "EWS Client Secret går ut för $($row.CustomerName) (ID: $($row.CustomerId))"
        $body = @"
Hej,

EWS Client Secret för kunden $($row.CustomerName) (ID: $($row.CustomerId)) går ut $($row.ExpireDate).

Vänligen förnya den så snart som möjligt.

Med vänliga hälsningar,  
Helpdesk Support
"@
        $recipient = $mailTo  # Replace with actual customer email lookup if needed

        Send-GraphEmail -token $token -recipient $recipient -subject $subject -body $body
        Write-Host "Mejl skickat till $recipient för kund: $($row.CustomerName) (ID: $($row.CustomerId))"
    }
} else {
    Write-Host "Inget mejl skickades, inga EWS-klienthemligheter är nära utgång."
}