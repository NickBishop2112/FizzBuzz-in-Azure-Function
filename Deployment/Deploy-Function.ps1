Param(
    [switch]$new,
    [switch]$remove
)

#Requires -RunAsAdministrator

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
$script:location = "UK South"
$script:storageAccount = "storagequeuehaskell"
if ($new.IsPresent)
{
    new-application `
        -resourceGroup $script:resourceGroup `
        -keyVault $script:keyVault `
        -location $script:location `
        -domainNameServiceName $script:domainNameServiceName `
        -templateFilePath (join-path -Path $PSScriptRoot -ChildPath 'template.json') `
        -parametersFilePath (join-path -Path $PSScriptRoot -ChildPath 'parameters.json') `
        -storageAccount $script:storageAccount
}

if ($remove.IsPresent)
{
    remove-application `
        -resourceGroup $script:resourceGroup `
        -KeyVaultName $script:keyVault `
        -location $script:location `
        -domainNameServiceName $script:domainNameServiceName `
        -storageAccount $script:storageAccount
}