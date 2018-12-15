Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

Import-Module -Name  ".\Deploy-Functions.psm1" -Force

if (-not (Test-UserLoggedOn))
{
    Write-Warning 'Log on to Azure'
    Connect-AzureRmAccount
}

New-AzureResourceGroup `
    -name 'Haskell-Functions' `
    -location 'West Europe'

Remove-AzureResourceGroup `
    -name 'Haskell-Functions' `
    -location 'West Europe'