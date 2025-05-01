resource "google_cloud_run_service" "service" {
  name     = var.service_name
  location = data.google_client_config.current.region

  template {
    spec {
      containers {
        image = local.image_uri
        
        resources {
          limits = {
            cpu    = var.cloud_run.cpu
            memory = var.cloud_run.memory
          }
        }

        dynamic "env" {
          for_each = var.environment.variables
          content {
            name  = env.key
            value = env.value
          }
        }
      }
      timeout_seconds = var.cloud_run.timeout_seconds
    }

    metadata {
      annotations = {
        "autoscaling.knative.dev/maxScale" = var.cloud_run.max_instances
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }

  depends_on = [google_artifact_registry_repository_iam_member.docker_pusher]
}

# Make the service publicly accessible
resource "google_cloud_run_service_iam_member" "public_access" {
  location = google_cloud_run_service.service.location
  service  = google_cloud_run_service.service.name
  role     = "roles/run.invoker"
  member   = "allUsers"
}