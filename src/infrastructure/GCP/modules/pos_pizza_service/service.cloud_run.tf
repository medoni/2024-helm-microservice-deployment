module "pizza_service" {
  source = "../gcp_cloud_run_based_on_dockerfile"
  
  service_name = "${var.project.short}-${var.env.short}-pizza-service"

  cloud_run = {
    timeout_seconds = 300
    memory          = "512Mi"
    cpu             = "1"
    max_instances   = 10
  }

  docker = {
    image_name = "pizza-service"
    image_tag  = var.service_version
    build = {
      dockerfile = "backend/Deployables/PizzaService.Gcp/Dockerfile"
      context    = "${path.root}/../../../../"
      build_arg  = {
        BUILD_VERSION = var.service_version
        BUILD_DATE    = var.build_date
        GIT_SHA       = var.build_git_sha
      }
    }
  }

  environment = {
    variables = {
      "Logging__LogLevel__Default" = "Debug"
      "Swagger__Enabled"           = "True"
      "Gcp__FireStore__ProjectId"  = data.google_project.current.project_id
      "Gcp__FireStore__Database"   = google_firestore_database.database.name
    }
  }

  logging = {
    retention_in_days = 14
  }
}

# Add Firestore permissions to the Cloud Run service account
resource "google_project_iam_member" "firestore_user" {
  project = data.google_project.current.project_id
  role    = "roles/datastore.user"
  member  = "serviceAccount:${module.pizza_service.service_account_email}"
}

# Additional Firestore data owner permissions
resource "google_project_iam_member" "firestore_data_owner" {
  project = data.google_project.current.project_id
  role    = "roles/datastore.owner"
  member  = "serviceAccount:${module.pizza_service.service_account_email}"
}