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

function New-SelfSignedCertificateForDevelopment
(
    [Parameter(Mandatory=$true)][string]$dnsName
)
{
    $filePath = "$PSScriptRoot\$dnsName.pfx"

    $certificate =
        New-SelfSignedCertificate `
            -DnsName $DnsName `
            -CertStoreLocation Cert:\CurrentUser\My `
            -KeySpec KeyExchange

    $generatedPassword = [System.Web.Security.Membership]::GeneratePassword(20,5)

    $null =
        Export-PfxCertificate `
            -Cert (Get-ChildItem -Path "cert:\CurrentUser\My\$($certificate.Thumbprint)") `
            -FilePath $filePath `
            -Password (ConvertTo-SecureString $generatedPassword -AsPlainText -Force)

    [hashtable]$return = @{}
    $return.ThumbPrint = $certificate.Thumbprint
    $return.GeneratedPassword = $generatedPassword
    $return.FilePath = $filePath

    Write-Output ('Created self-signed certificate, thumbprint = ''{0}'', Password = ''{1}'', path = ''{2}''' -f $return.Thumbprint, $return.GeneratedPassword, $return.FilePath)
    return $return
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