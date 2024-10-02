#Requires -Version 5.0

# Install-Module AzureAD -Scope CurrentUser -Force

# Install-Module Az -Force    
# Import-Module Az.Websites -Force

$ErrorActionPreference = "Stop"
Set-StrictMode -Version 3

#####################################################
# The steps are ok but our DevOps agents do not have 
# the required permissions to create the App Registration
# in Entra ID, so this is a manual process for now!
#####################################################


$AppRegName = "appreg-nis2024-01"
$AppId = "api://$AppRegName"

#####################################################
# Create App Registartion

# Check if the application already exists
#$appRegistration = Get-AzureADApplication | Where-Object { $_.DisplayName -contains $AppRegName }
#$appRegistration = Get-AzADApplication -SearchString $AppRegName | Where-Object { $_.IdentifierUris -contains $AppId }
$appRegistration = Get-AzADApplication -DisplayName $AppRegName

if ($appRegistration) {
    Write-Output "Application $AppRegName already exists with ID: $($appRegistration.AppId)"
} else {
    # Create the application if it does not exist
    #$appRegistration = New-AzureADApplication -DisplayName $AppRegName -IdentifierUris $AppId
    $appRegistration = New-AzADApplication -DisplayName $AppRegName -IdentifierUris $AppId
    Write-Output "New App Registration created with ID: $($appRegistration.AppId)"

    $servicePrincipal = New-AzADServicePrincipal -ApplicationId $appRegistration.AppId
    Write-Output "New Enterprise Application created with ID: $($servicePrincipal.Id)"
}


#####################################################
# Create WeatherContributor role in the application

# Check if the role already exists
$appRegistration = Get-AzADApplication -DisplayName $AppRegName
$servicePrincipal = Get-AzADServicePrincipal -ApplicationId $appRegistration.AppId
$currentRoles = $servicePrincipal.AppRole
$roleExists = $currentRoles | Where-Object { $_.DisplayName -eq "Weather Contributor" }

if ($roleExists) {
    $contributorRoleId = $roleExists.Id
    Write-Output "Role already exists: $($roleExists.DisplayName)"
} else {

    # Role does not exist, add it to the current roles and update the application
    $contributorRole = [Microsoft.Azure.PowerShell.Cmdlets.Resources.MSGraph.Models.ApiV10.MicrosoftGraphAppRole]::new()
    $contributorRole.Id = New-Guid
    $contributorRole.DisplayName = "Weather Contributor"
    $contributorRole.Description = "Allow reading and updating of weather."
    $contributorRole.Value = "WeatherContributor"
    $contributorRole.IsEnabled = $true
    $contributorRole.AllowedMemberType = @("Application")
    $contributorRoleId = $contributorRole.Id

    $newRoles = $appRegistration.AppRole + $contributorRole
    Update-AzADApplication -ObjectId $appRegistration.Id -AppRole $newRoles

    Write-Output "Role WeatherContributor added successfully: $($role.DisplayName)"
}



#####################################################
# Create WeatherReader role in the application

# Check if the role already exists
$appRegistration = Get-AzADApplication -DisplayName $AppRegName
$servicePrincipal = Get-AzADServicePrincipal -ApplicationId $appRegistration.AppId
$currentRoles = $servicePrincipal.AppRole
$roleExists = $currentRoles | Where-Object { $_.DisplayName -eq "Weather Reader" }

if ($roleExists) {
    $readerRoleId = $roleExists.Id
    Write-Output "Role already exists: $($roleExists.DisplayName)"
} else {

    # Role does not exist, add it to the current roles and update the application
    $readerRole = [Microsoft.Azure.PowerShell.Cmdlets.Resources.MSGraph.Models.ApiV10.MicrosoftGraphAppRole]::new()
    $readerRole.Id = New-Guid
    $readerRole.DisplayName = "Weather Reader"
    $readerRole.Description = "Allow reading weather."
    $readerRole.Value = "WeatherReader"
    $readerRole.IsEnabled = $true
    $readerRole.AllowedMemberType = @("Application")
    $readerRoleId = $readerRole.Id

    $newRoles = $appRegistration.AppRole + $readerRole
    Update-AzADApplication -ObjectId $appRegistration.Id -AppRole $newRoles

    Write-Output "Role WeatherReader added successfully: $($role.DisplayName)"

}

#####################################################
# Retrieve your managed identity
$appService = Get-AzWebApp -ResourceGroupName "nis1" -Name "web-nis2024-01"
$principalId = $appService.Identity.PrincipalId

Write-Output "MID= $($principalId)"

#####################################################
# Assign the role to the managed identity (Role-based access control)

# ObjectId of the Managed Identity (Identity in Azure portal) assigned to the web application (app-alz-devtest-sc-idmpapi-preview-datasync-01) that should get the role.
$WebAppManagedIdentity = $principalId

# ObjectId of the Enterprise Application (appreg-umc-ext-prd-cr-demo-rg01-website-idmpapi), the application that is beeing called.
$EntAppObjectId = $servicePrincipal.Id


# Id property of the custom app registration role, taken from the Manifest JSON shown in the Azure Portal.
$RoleIdInAppReg = $contributorRoleId

# Execute Graph API call using Azure CLI
az rest -m POST -u https://graph.microsoft.com/v1.0/servicePrincipals/$WebAppManagedIdentity/appRoleAssignments -b "{'principalId': '$WebAppManagedIdentity', 'resourceId': '$EntAppObjectId', 'appRoleId': '$RoleIdInAppReg'}"

# Id property of the custom app registration role, taken from the Manifest JSON shown in the Azure Portal.
$RoleIdInAppReg = $readerRoleId

# Execute Graph API call using Azure CLI
az rest -m POST -u https://graph.microsoft.com/v1.0/servicePrincipals/$WebAppManagedIdentity/appRoleAssignments -b "{'principalId': '$WebAppManagedIdentity', 'resourceId': '$EntAppObjectId', 'appRoleId': '$RoleIdInAppReg'}"
