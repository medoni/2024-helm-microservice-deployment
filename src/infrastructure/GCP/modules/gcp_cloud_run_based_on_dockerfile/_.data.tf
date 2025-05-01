data "google_client_config" "current" {}

data "google_project" "current" {}

# Import reference to the artifact registry API service
data "google_project_service" "artifact_registry_api" {
  service = "artifactregistry.googleapis.com"
}