Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

Import-Module -Name  ".\Deploy-Functions.psm1" -Force

$script:resourceGroup = 'haskellFunctions-dev-rg'
$script:keyVault = 'haskellFunctions-kv'
$script:domainNameServiceName = 'haskkellfunctions'

if (Test-UserLoggedOn)
{
    Write-Output 'Already logged in'
}
else
{
    Write-Output 'Log on to Azure'
    Connect-AzureRmAccount
}

$script:location = (Get-AzureRmLocation | Where-Object {$_.location.StartsWith('uk')} | select-object -first 1).DisplayName

new-application `
    -resourceGroup $script:resourceGroup `
    -keyVault $script:keyVault `
    -location $script:location `
    -domainNameServiceName $script:domainNameServiceName

exit

remove-application `
    -resourceGroup $script:resourceGroup `
    -location $script:location