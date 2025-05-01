output "service_url" {
  description = "The URL where the Cloud Run service is available"
  value       = google_cloud_run_service.service.status[0].url
}

output "service_name" {
  description = "The name of the Cloud Run service"
  value       = google_cloud_run_service.service.name
}

output "image_uri" {
  description = "The URI of the container image deployed to Cloud Run"
  value       = local.image_uri
}

output "region" {
  description = "The region where the service is deployed"
  value       = data.google_client_config.current.region
}

output "project_id" {
  description = "The ID of the Google Cloud Project where the service is deployed"
  value       = data.google_project.current.project_id
}

output "service_account_email" {
  description = "The email of the service account used by the Cloud Run service"
  value       = google_service_account.docker_pusher.email
}