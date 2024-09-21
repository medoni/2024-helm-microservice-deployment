# 
# This scripts adds neceassry project and applications into argo cd
#
# usage:
#   src/infrastructure/ArgoCD/bootstrap.ps1

[CmdletBinding()]
param()

$kubeNamespace = "2024-helm-microservice-deployment"
$scriptDirectory = $PSScriptRoot


function Invoke-Main() {
    Check-KubeCtlCommand
    Create-KubeCtlNs
    Apply-ArgoProject
    Apply-ArgoApplication

    Create-Secrets
}

function Create-KubeCtlNs() {
    kubectl create namespace $kubeNamespace --dry-run=client -o yaml | kubectl apply -f -
    if ($LASTEXITCODE -ne 0) {
        Handle-Error "Failed to create namespace '$kubeNamespace'."
    }
}

function Apply-ArgoProject() {
    $yamlFile = "$scriptDirectory/argoproject.yaml"
    Write-Host "Applying ArgoCD Project configuration from '$yamlFile'..."
    kubectl apply -f $yamlFile
    if ($LASTEXITCODE -ne 0) {
        Handle-Error "Failed to apply $yamlFile."
    } else {
        Write-Host "ArgoCD Project configuration applied successfully."
    }
}

function Apply-ArgoApplication() {
    $yamlFile = "$scriptDirectory/argoapplication.yaml"
    Write-Host "Applying ArgoCD Application configuration from '$yamlFile'..."
    kubectl apply -f $yamlFile
    if ($LASTEXITCODE -ne 0) {
        Handle-Error "Failed to apply $yamlFile."
    } else {
        Write-Host "ArgoCD Application configuration applied successfully."
    }
}

function Create-Secrets() {
    $password = Generate-Secret 20

    kubectl create secret generic postgres-db-secrets `
        --save-config `
        --dry-run=client `
        --from-literal=username=postgres `
        --from-literal=password="$password" `
        -n $kubeNamespace `
        -o yaml `
        | kubectl apply -f -
    
    if ($LASTEXITCODE -ne 0) {
        Handle-Error "Failed to create secrets."
    } else {
        Write-Host "Secrets for Postgres successfully created."
    }
}

function Generate-Secret() {
    param(
        [int]$Length = 20
    )
    $chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+'
    -join (1..$Length | ForEach-Object { $chars[(Get-Random -Minimum 0 -Maximum $chars.Length)] })
}

function Check-KubeCtlCommand() {
    if (-not (Get-Command kubectl -ErrorAction SilentlyContinue)) {
        Handle-Error "kubectl is not installed or not in the system's PATH."
    }
}

function Handle-Error {
    param(
        [string]$Message
    )
    Write-Host "ERROR: $Message" -ForegroundColor Red
    exit 1
}


Invoke-Main