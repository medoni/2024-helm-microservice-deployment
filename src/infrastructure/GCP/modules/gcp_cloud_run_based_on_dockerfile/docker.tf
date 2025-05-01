resource "google_artifact_registry_repository" "docker_repo" {
  provider = google
  location = data.google_client_config.current.region
  
  repository_id = var.docker.image_name
  description   = "Docker repository for ${var.service_name} images"
  format        = "DOCKER"
}

resource "google_artifact_registry_repository_iam_member" "docker_pusher" {
  provider = google
  
  location   = google_artifact_registry_repository.docker_repo.location
  repository = google_artifact_registry_repository.docker_repo.name
  role       = "roles/artifactregistry.writer"
  member     = "serviceAccount:${data.google_project.current.number}@cloudbuild.gserviceaccount.com"
}

module "docker_image" {
  source = "terraform-google-modules/container-image/google"
  
  image_name   = var.docker.image_name
  artifact_registry = "us-docker.pkg.dev/${data.google_project.current.project_id}/${var.docker.image_name}"
  
  source_dir   = var.docker.build.context
  dockerfile   = var.docker.build.dockerfile
  build_args   = var.docker.build.build_arg
  
  image_tag    = var.docker.image_tag
}