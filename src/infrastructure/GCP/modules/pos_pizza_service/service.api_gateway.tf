# Enable API Gateway API
resource "google_project_service" "apigateway_api" {
  project = data.google_project.current.project_id
  service = "apigateway.googleapis.com"
  
  disable_on_destroy = false
}

# Enable Service Management API required by API Gateway
resource "google_project_service" "servicemanagement" {
  project = data.google_project.current.project_id
  service = "servicemanagement.googleapis.com"

  disable_on_destroy = false
}

# Enable Service Control API required by API Gateway
resource "google_project_service" "servicecontrol" {
  project = data.google_project.current.project_id
  service = "servicecontrol.googleapis.com"

  disable_on_destroy = false
}

# Create a service account for API Gateway
resource "google_service_account" "api_gateway_sa" {
  account_id   = "apigw-${var.project.short}-${var.env.short}"
  display_name = "API Gateway Service Account"
  description  = "Service account for API Gateway"
}

# Grant roles to the service account
resource "google_project_iam_member" "api_gateway_invoker" {
  project = data.google_project.current.project_id
  role    = "roles/run.invoker"
  member  = "serviceAccount:${google_service_account.api_gateway_sa.email}"
}

resource "google_project_iam_member" "api_gateway_servicecontrol" {
  project = data.google_project.current.project_id
  role    = "roles/servicemanagement.serviceController"
  member  = "serviceAccount:${google_service_account.api_gateway_sa.email}"
}

# Create an API Gateway API
resource "google_api_gateway_api" "pizza_api" {
  provider = google-beta
  api_id   = "${var.project.short}-${var.env.short}-api"
  display_name = "Pizza Ordering API"
  
  depends_on = [
    google_project_service.apigateway_api,
    google_project_service.servicemanagement,
    google_project_service.servicecontrol
  ]
}

# Deploy the API Gateway
resource "google_api_gateway_gateway" "pizza_gateway" {
  provider     = google-beta
  api_config   = google_api_gateway_api_config.pizza_api_config.id
  gateway_id   = "${var.project.short}-${var.env.short}-gw1"
  display_name = "Pizza API Gateway"
  
  depends_on = [google_api_gateway_api_config.pizza_api_config]
}

# Output the API Gateway URL
output "api_gateway_url" {
  value       = google_api_gateway_gateway.pizza_gateway.default_hostname
  description = "The URL of the API Gateway"
}