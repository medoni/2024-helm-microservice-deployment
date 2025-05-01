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

# Define the API Gateway configuration
resource "google_api_gateway_api_config" "pizza_api_config" {
  provider      = google-beta
  api           = google_api_gateway_api.pizza_api.api_id
  api_config_id = "${var.project.short}-${var.env.short}-config"
  display_name  = "Pizza API Config"
  
  openapi_documents {
    document {
      path = "spec.yaml"
      contents = base64encode(<<-EOT
        swagger: '2.0'
        info:
          title: Pizza Ordering API
          description: API for Pizza Ordering System
          version: 1.0.0
        schemes:
          - https
        produces:
          - application/json
        paths:
          /api/menus:
            get:
              summary: Get menu items
              operationId: getMenuItems
              x-google-backend:
                address: ${module.pizza_service.service_url}/api/menus
              responses:
                '200':
                  description: A list of menu items
          /api/orders:
            get:
              summary: Get customer orders
              operationId: getOrders
              x-google-backend:
                address: ${module.pizza_service.service_url}/api/orders
              responses:
                '200':
                  description: A list of orders
            post:
              summary: Create a new order
              operationId: createOrder
              x-google-backend:
                address: ${module.pizza_service.service_url}/api/orders
              responses:
                '201':
                  description: Order created successfully
          /api/carts:
            get:
              summary: Get customer cart
              operationId: getCart
              x-google-backend:
                address: ${module.pizza_service.service_url}/api/carts
              responses:
                '200':
                  description: Customer cart
            post:
              summary: Add item to cart
              operationId: addItemToCart
              x-google-backend:
                address: ${module.pizza_service.service_url}/api/carts
              responses:
                '200':
                  description: Item added to cart
        EOT
      )
    }
  }
  
  gateway_config {
    backend_config {
      google_service_account = google_service_account.api_gateway_sa.email
    }
  }
  
  lifecycle {
    create_before_destroy = true
  }
  
  depends_on = [
    google_api_gateway_api.pizza_api,
    google_project_iam_member.api_gateway_invoker,
    google_project_iam_member.api_gateway_servicecontrol
  ]
}

# Deploy the API Gateway
resource "google_api_gateway_gateway" "pizza_gateway" {
  provider     = google-beta
  api_config   = google_api_gateway_api_config.pizza_api_config.id
  gateway_id   = "${var.project.short}-${var.env.short}-gateway"
  display_name = "Pizza API Gateway"
  
  depends_on = [google_api_gateway_api_config.pizza_api_config]
}

# Output the API Gateway URL
output "api_gateway_url" {
  value       = google_api_gateway_gateway.pizza_gateway.default_hostname
  description = "The URL of the API Gateway"
}