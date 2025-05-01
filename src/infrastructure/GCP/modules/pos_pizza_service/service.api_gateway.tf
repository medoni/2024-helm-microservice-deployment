resource "google_endpoints_service" "pizza_api" {
  service_name   = "pizza-api-${var.env.short}.endpoints.${data.google_project.current.project_id}.cloud.goog"
  project        = data.google_project.current.project_id
  openapi_config = <<EOF
swagger: '2.0'
info:
  title: Pizza Ordering API
  description: API for Pizza Ordering System
  version: 1.0.0
host: "pizza-api-${var.env.short}.endpoints.${data.google_project.current.project_id}.cloud.goog"
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
EOF
}

resource "google_project_service" "endpoints" {
  project = data.google_project.current.project_id
  service = "endpoints.googleapis.com"

  disable_on_destroy = false
}

resource "google_project_service" "servicemanagement" {
  project = data.google_project.current.project_id
  service = "servicemanagement.googleapis.com"

  disable_on_destroy = false
}

resource "google_project_service" "servicecontrol" {
  project = data.google_project.current.project_id
  service = "servicecontrol.googleapis.com"

  disable_on_destroy = false
}

# Create a service account for the Cloud Endpoints
resource "google_service_account" "endpoints_service_account" {
  account_id   = "pizza-endpoints-sa"
  display_name = "Pizza API Endpoints Service Account"
}

# Grant necessary permissions to the service account
resource "google_project_iam_member" "endpoints_service_account_role" {
  project = data.google_project.current.project_id
  role    = "roles/servicemanagement.serviceController"
  member  = "serviceAccount:${google_service_account.endpoints_service_account.email}"
}

# Create a Cloud Endpoints service attachment to expose the API
resource "google_compute_global_address" "endpoints_ip" {
  name         = "pizza-endpoints-ip"
  description  = "Global IP for Pizza API Endpoints"
  address_type = "EXTERNAL"
}

output "endpoints_url" {
  value       = "https://${google_endpoints_service.pizza_api.service_name}"
  description = "The URL of the Cloud Endpoints API"
}