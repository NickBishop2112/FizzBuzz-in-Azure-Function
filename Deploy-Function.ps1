Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

Import-Module -Name  ".\Deploy-Functions.psm1" -Force


if (Test-UserLoggedOn)
{
    Write-Output 'Already logged in'
}
else
{
    Write-Output 'Log on to Azure'
    Connect-AzureRmAccount
}

$script:resourceGroup = 'haskellFunctions-dev-rg'
$script:keyVault = 'haskellFunctions-kv'
$script:domainNameServiceName = 'haskkellfunctions'
$script:location = (Get-AzureRmLocation | Where-Object {$_.location.StartsWith('uk')} | select-object -first 1).DisplayName

remove-application `
    -resourceGroup $script:resourceGroup `
    -KeyVaultName $script:keyVault `
    -location $script:location
exit

new-application `
    -resourceGroup $script:resourceGroup `
    -keyVault $script:keyVault `
    -location $script:location `
    -domainNameServiceName $script:domainNameServiceName `
    -templateFilePath (join-path -Path $PSScriptRoot -ChildPath 'template.json') `
    -parametersFilePath (join-path -Path $PSScriptRoot -ChildPath 'parameters.json')

remove-application `
    -resourceGroup $script:resourceGroup `
    -location $script:location