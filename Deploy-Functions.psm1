Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

function New-AzureResourceGroup
(
    [Parameter(Mandatory=$true)][string]$name,
    [Parameter(Mandatory=$true)][string]$location
)
{
    $resourceGroup = (Get-AzureRmResourceGroup -Name $Name -Location $Location -ErrorAction Ignore)

    if (!$resourceGroup)
    {
        $null = `
            New-AzureRmResourceGroup `
                -Name $name `
                -Location $location
        Write-Output  "'$name' Resource Group located in '$location' has been created"
    }
    else
    {
        Write-Warning "'$name' Resource Group located in '$location' already exists"
    }
}

function Remove-AzureResourceGroup
(
    [Parameter(Mandatory=$true)][string]$name,
    [Parameter(Mandatory=$true)][string]$location
)
{
    $resourceGroup = (Get-AzureRmResourceGroup -Name $Name -Location $Location -ErrorAction Ignore)

    if ($resourceGroup)
    {
        Write-Output  "Removing '$name' Resource Group located in '$location'"
        $null = Remove-AzureRmResourceGroup -Name $name -Force
        Write-Output  "'$name' Resource Group located in '$location' has been removed"
    }
    else
    {
        Write-Warning "'$name' Resource Group located in '$location' does not exist"
    }
}

function New-AzureKeyVault
(
    [Parameter(Mandatory=$true)][string]$name,
    [Parameter(Mandatory=$true)][string]$resourceGroupName,
    [Parameter(Mandatory=$true)][string]$location
)
{
    $keyVault = Get-AzureRmKeyVault -VaultName $Name -ErrorAction Ignore

    if ($null -eq $keyVault)
    {
        $null =
            New-AzureRmKeyVault  `
                -name $name `
                -resourceGroupName $resourceGroupName `
                -location $location `
                -EnabledForDeployment
        Write-Output  "'$name' Key Vault in '$resourceGroupName' Resource Group located in '$location' has been created"
    }
    else
    {
        Write-Warning  "'$name' Key Vault in '$resourceGroupName' Resource Group located in '$location' already exists"
    }
}

function Remove-AzureKeyVault
(
    [Parameter(Mandatory=$true)][string]$name,
    [Parameter(Mandatory=$true)][string]$resourceGroupName,
    [Parameter(Mandatory=$true)][string]$location
)
{
    $keyVault = Get-AzureRmKeyVault -VaultName $Name -ErrorAction Ignore

    if ($null -ne $keyVault)
    {
        Write-Output  "Removing'$name' Key Vault in '$resourceGroupName' Resource Group located in '$location'"
        $null =
            Remove-AzureRmKeyVault `
                -VaultName $name `
                -resourceGroupName $resourceGroupName `
                -location $location `
                -Force -PassThru

        Write-Output  "'$name' Key Vault in '$resourceGroupName' Resource Group located in '$location' has been removed"
    }
    else
    {
        Write-Warning  "'$name' Key Vault in '$resourceGroupName' Resource Group located in '$location' does not exist"
    }
}

function Test-UserLoggedOn()
{
    return $null -ne (Get-AzureRmContext).Account
}