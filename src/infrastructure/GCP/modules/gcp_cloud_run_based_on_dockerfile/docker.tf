resource "google_artifact_registry_repository" "docker_repo" {
  provider = google
  location = data.google_client_config.current.region
  
  repository_id = var.docker.image_name
  description   = "Docker repository for ${var.service_name} images"
  format        = "DOCKER"
  
  # Use project-level API enablement from pos_pizza_service module
}

# Create a service account for Docker operations
resource "google_service_account" "docker_pusher" {
  account_id   = "docker-pusher-${substr(md5(var.service_name), 0, 8)}"
  display_name = "Docker Service Account for ${var.service_name}"
  description  = "Service account for pushing Docker images to Artifact Registry"
}

resource "google_artifact_registry_repository_iam_member" "docker_pusher" {
  provider = google
  
  location   = google_artifact_registry_repository.docker_repo.location
  repository = google_artifact_registry_repository.docker_repo.name
  role       = "roles/artifactregistry.writer"
  member     = "serviceAccount:${google_service_account.docker_pusher.email}"
}

# Build and push Docker image using null_resource and local-exec
resource "null_resource" "docker_build_push" {
  triggers = {
    dockerfile_content = filesha256("${var.docker.build.context}/${var.docker.build.dockerfile}")
    docker_context    = var.docker.build.context
    docker_tag        = var.docker.image_tag
  }

  provisioner "local-exec" {
    interpreter = ["PowerShell", "-Command"]
    command = <<EOT
      # Build the Docker image
      docker build -t ${var.docker.image_name}:${var.docker.image_tag} `
        ${join(" ", [for key, value in var.docker.build.build_arg : "--build-arg ${key}=${value}"])} `
        -f ${var.docker.build.context}/${var.docker.build.dockerfile} `
        ${var.docker.build.context}

      # Configure Docker to use Google Cloud credentials
      gcloud auth configure-docker ${data.google_client_config.current.region}-docker.pkg.dev --quiet

      # Tag the image for Artifact Registry
      docker tag ${var.docker.image_name}:${var.docker.image_tag} `
        ${data.google_client_config.current.region}-docker.pkg.dev/${data.google_project.current.project_id}/${var.docker.image_name}/${var.docker.image_name}:${var.docker.image_tag}

      # Push the image to Artifact Registry
      docker push `
        ${data.google_client_config.current.region}-docker.pkg.dev/${data.google_project.current.project_id}/${var.docker.image_name}/${var.docker.image_name}:${var.docker.image_tag}
    EOT
  }

  depends_on = [google_artifact_registry_repository.docker_repo]
}