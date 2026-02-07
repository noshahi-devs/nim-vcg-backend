
$ErrorActionPreference = "Stop"
# Client not needed for Invoke-WebRequest

# Ignore SSL errors
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = { $true }

$baseUrl = "https://localhost:7225/api/users"
$email = "test_login_alert_" + (Get-Date -Format "yyyyMMddHHmmss") + "@noshahi.com"
$password = "TestPass123!"
$username = "TestUser" + (Get-Date -Format "HHmmss")

Write-Host "1. Registering User: $email"
$regBody = @{
    email = $email
    username = $username
    password = $password
    role = @("Teacher")
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/register" -Method Post -Body $regBody -ContentType "application/json"
    Write-Host "   Registration Response: $($response.StatusCode)"
}
catch {
    Write-Host "   Registration Failed or User Exists: $($_.Exception.Message)"
}

Write-Host "`n2. Logging In..."
$loginBody = @{
    email = $email
    password = $password
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/login" -Method Post -Body $loginBody -ContentType "application/json"
    Write-Host "   Login Successful: $($response.StatusCode)"
    Write-Host "   Token received."
    Write-Host "`nCHECK BACKEND TERMINAL FOR EMAIL LOGS!"
}
catch {
    Write-Host "   Login Failed: $($_.Exception.Message)"
    # Write-Host "   Body: $($_.ErrorDetails.Message)"
}
