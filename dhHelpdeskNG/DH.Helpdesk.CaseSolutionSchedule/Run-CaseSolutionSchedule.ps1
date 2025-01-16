# Ytterbygg appPath
#$appPath = "D:\Services\HelpdeskCaseSolutionSchedule\DH.Helpdesk.CaseSolutionSchedule.exe"
#Local appPath
$appPath = "C:\Repos\DHSolution\dh-helpdesk\dhHelpdeskNG\DH.Helpdesk.CaseSolutionSchedule\bin\Debug\DH.Helpdesk.CaseSolutionSchedule.exe"
# Ytterbygg working directory
#$workingDirectory = "D:\Services\HelpdeskCaseSolutionSchedule"
# Local working directory
$workingDirectory = "C:\Repos\DHSolution\dh-helpdesk\dhHelpdeskNG\DH.Helpdesk.CaseSolutionSchedule\bin\Debug"

# Define the arguments
$dateAndTime = "2025-01-14 07:05"
$workMode = 1 #Only logging casesolutions that should be created at the time above


# Ensure the working directory exists
if (-not (Test-Path -Path $workingDirectory)) {
    Write-Host "Working directory does not exist: $workingDirectory" -ForegroundColor Red
    exit 1
}

# Change to the working directory
Set-Location -Path $workingDirectory

# Run the application
try {
    & $appPath $dateAndTime $workMode
    Write-Host "CaseSolutionSchedule ran successfully." -ForegroundColor Green
} catch {
    Write-Host "Error running CaseSolutionSchedule: $_" -ForegroundColor Red
    exit 1
}
