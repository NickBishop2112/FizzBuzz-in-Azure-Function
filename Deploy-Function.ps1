Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

Import-Module -Name  ".\Deploy-Functions.psm1" -Force

$resourceGroup = 'haskellFunctions-dev-rg'
$keyVault = 'haskellFunctions-kv'
$location = (Get-AzureRmLocation | ? {$_.location.StartsWith('uk')} | select-object -first 1).DisplayName


if (Test-UserLoggedOn)
{
    Write-Output 'Already logged in'
}
else
{
    Write-Output 'Log on to Azure'
    Connect-AzureRmAccount
}

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