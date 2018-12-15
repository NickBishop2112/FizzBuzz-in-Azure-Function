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

function Test-UserLoggedOn()
{
    $resourceManagerContext = Get-AzureRmContext

    return $null -ne $resourceManagerContext
}