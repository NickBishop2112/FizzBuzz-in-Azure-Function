Import-Module -Name .\Deploy-Functions.psm1

New-AzureResourceGroup `
    -name 'Haskell-Functions' `
    -location 'West Europe'