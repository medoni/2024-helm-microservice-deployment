# 
# This builds all necessary deployables and installs / updates to the local environment
#
# usage:
#   src/build-and-deploy-dev-local.ps1

[CmdletBinding()]
param()

$kubeNamespace     = "2024-helm-microservice-deployment"
$imageTagName      = "latest" 
$containerRegistry = "registry.mycluster.localhost"
$scriptDirectory = $PSScriptRoot


function Invoke-Main() {
    BuildAndPush-DockerImage "$scriptDirectory" "$scriptDirectory/backend/Deployables/PizzaService.AspNetCore/Dockerfile" pizza-service
    BuildAndPush-DockerImage "$scriptDirectory" "$scriptDirectory/backend/Infrastructure/POS.Persistence.PostgreSql.DbMigrations/Dockerfile" pizza-db-migrations
    BuildAndPush-DockerImage "$scriptDirectory" "$scriptDirectory/backend/Infrastructure/POS.Persistence.DbSeed/Dockerfile" pizza-db-seed
    
    Apply-Helm-Chart ./infrastructure/Kubernetes/helm/dev-local/PizzaService/

    Force-Redeployment
}

function BuildAndPush-DockerImage {
    param (
        [string]$buildContext,
        [string]$dockerfile,
        [string]$imageName
    )

    $fullImageName = "$containerRegistry/$($imageName):$imageTagName"

    docker build -t $fullImageName -f $dockerfile $buildContext
    if ($LASTEXITCODE -ne 0) { Handle-Error "Failed to build docker image '$fullImageName'" }

    docker push $fullImageName
    if ($LASTEXITCODE -ne 0) { Handle-Error "Failed to push docker image '$fullImageName'" }
}

function Apply-Helm-Chart
{
    param (
        [string]$helmChartPath
    )

    helm template $helmChartPath | kubectl apply -n $kubeNamespace -f -
    if ($LASTEXITCODE -ne 0) { Handle-Error "Failed to push docker image '$fullImageName'" }
}

function Force-Redeployment
{
    kubectl rollout restart deployment -n $kubeNamespace
    if ($LASTEXITCODE -ne 0) { Handle-Error "Failed to push docker image '$fullImageName'" }
}

function Handle-Error {
    param(
        [string]$Message
    )
    Write-Host "ERROR: $Message" -ForegroundColor Red
    exit 1
}

Invoke-Main
