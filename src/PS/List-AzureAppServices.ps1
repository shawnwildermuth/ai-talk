[CmdletBinding()]
param(
    [switch]$CurrentSubscriptionOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Ensure Azure CLI is available.
if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    throw "Azure CLI ('az') is not installed or not on PATH. Install it from: https://aka.ms/azure-cli"
}

function Invoke-AzCliJson {
    param(
        [Parameter(Mandatory = $true)]
        [string[]]$Arguments
    )

    $output = & az @Arguments --only-show-errors --output json 2>$null
    if ($LASTEXITCODE -ne 0) {
        return $null
    }

    if ([string]::IsNullOrWhiteSpace($output)) {
        return $null
    }

    return $output | ConvertFrom-Json
}

# Ensure there is an authenticated Azure CLI context.
$account = Invoke-AzCliJson -Arguments @('account', 'show')
if (-not $account) {
    Write-Host 'No active Azure CLI login found. Opening sign-in...'
    & az login --only-show-errors --output none
    if ($LASTEXITCODE -ne 0) {
        throw 'Azure CLI login failed.'
    }

    $account = Invoke-AzCliJson -Arguments @('account', 'show')
    if (-not $account) {
        throw 'Unable to read Azure CLI account context after login.'
    }
}

$results = @()

if ($CurrentSubscriptionOnly) {
    Write-Host ("Querying App Services in current subscription: {0}" -f $account.name)

    $apps = Invoke-AzCliJson -Arguments @('webapp', 'list')
    if (-not $apps) {
        $apps = @()
    }

    foreach ($app in $apps) {
        $results += [pscustomobject]@{
            SubscriptionName = $account.name
            SubscriptionId   = $account.id
            ResourceGroup    = $app.resourceGroup
            Name             = $app.name
            Location         = $app.location
            State            = $app.state
            Kind             = $app.kind
            DefaultHostName  = $app.defaultHostName
        }
    }
}
else {
    $subscriptions = Invoke-AzCliJson -Arguments @('account', 'list')
    if (-not $subscriptions) {
        throw 'Failed to retrieve subscriptions from Azure CLI.'
    }

    foreach ($subscription in $subscriptions) {
        Write-Host ("Querying App Services in subscription: {0}" -f $subscription.name)
        & az account set --subscription $subscription.id --only-show-errors
        if ($LASTEXITCODE -ne 0) {
            Write-Warning ("Skipping subscription '{0}' because context could not be set." -f $subscription.name)
            continue
        }

        $apps = Invoke-AzCliJson -Arguments @('webapp', 'list')
        if (-not $apps) {
            $apps = @()
        }

        foreach ($app in $apps) {
            $results += [pscustomobject]@{
                SubscriptionName = $subscription.name
                SubscriptionId   = $subscription.id
                ResourceGroup    = $app.resourceGroup
                Name             = $app.name
                Location         = $app.location
                State            = $app.state
                Kind             = $app.kind
                DefaultHostName  = $app.defaultHostName
            }
        }
    }
}

if (-not $results) {
    Write-Host 'No Azure App Services were found for the current account.'
    return
}

$results |
    Sort-Object SubscriptionName, ResourceGroup, Name |
    Format-Table SubscriptionName, ResourceGroup, Name, Location, State, DefaultHostName -AutoSize
