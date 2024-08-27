# Define the environment variable name and value
$envVarName = "RECAPTCHA_SECRET"
$envVarValue = "6Lf-hQIqAAAAAIB4cEX6xTWBUYdMiNq9me6hqcOB"

# Check if the environment variable exists at the machine level
$currentValue = [System.Environment]::GetEnvironmentVariable($envVarName, "Machine")

if ($null -eq $currentValue) {
    # If the variable does not exist, create it
    [System.Environment]::SetEnvironmentVariable($envVarName, $envVarValue, "Machine")
    Write-Host "Environment variable $envVarName set to $envVarValue at the Machine level."
} else {
    # If the variable exists, notify the user
    Write-Host "Environment variable $envVarName for Machine already exists with value: $currentValue"
}
