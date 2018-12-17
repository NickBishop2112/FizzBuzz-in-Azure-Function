Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

Import-Module -Name  ".\Deploy-Functions.psm1" -Force

$resourceGroup = 'haskellFunctions-dev-rg'
$keyVault = 'haskellFunctions-kv'

New-SelfSignedCertificateForDevelopment -dnsName 'haskkellfunctions'
exit

if (Test-UserLoggedOn)
{
    Write-Output 'Already logged in'
}
else
{
    Write-Output 'Log on to Azure'
    Connect-AzureRmAccount
}

$location = (Get-AzureRmLocation | Where-Object {$_.location.StartsWith('uk')} | select-object -first 1).DisplayName

New-AzureResourceGroup `
    -name $resourceGroup `
    -location $location

New-AzureKeyVault `
    -name $keyVault `
    -resourceGroupName $resourceGroup `
    -location $location

Remove-AzureKeyVault `
    -name $keyVault `
    -resourceGroupName $resourceGroup `
    -location $location

Remove-AzureResourceGroup `
    -name $resourceGroup `
    -location $location
