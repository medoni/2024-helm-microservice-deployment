# Enable required Google Cloud APIs for the project
resource "google_project_service" "artifact_registry_api" {
  project = data.google_project.current.project_id
  service = "artifactregistry.googleapis.com"
  
  disable_dependent_services = false
  disable_on_destroy = false
}

resource "google_project_service" "compute_api" {
  project = data.google_project.current.project_id
  service = "compute.googleapis.com"
  
  disable_dependent_services = false
  disable_on_destroy = false
}

resource "google_project_service" "firestore_api" {
  project = data.google_project.current.project_id
  service = "firestore.googleapis.com"
  
  disable_dependent_services = false
  disable_on_destroy = false
}

resource "google_project_service" "cloud_run_api" {
  project = data.google_project.current.project_id
  service = "run.googleapis.com"
  
  disable_dependent_services = false
  disable_on_destroy = false
}

resource "google_project_service" "pubsub_api" {
  project = data.google_project.current.project_id
  service = "pubsub.googleapis.com"
  
  disable_dependent_services = false
  disable_on_destroy = false
}