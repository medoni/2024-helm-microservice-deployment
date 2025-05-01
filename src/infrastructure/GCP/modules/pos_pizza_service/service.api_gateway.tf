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

resource "google_api_gateway_api" "api" {
  provider = google
  
  api_id       = "pizza-api-gateway"
  display_name = "Pizza API Gateway"
  depends_on   = [google_project_service.apigateway]
}

resource "google_project_service" "apigateway" {
  service = "apigateway.googleapis.com"

  disable_on_destroy = false
}

resource "google_api_gateway_api_config" "api_config" {
  provider = google
  
  api           = google_api_gateway_api.api.api_id
  api_config_id = "pizza-api-config-${var.env.short}"
  display_name  = "Pizza API Config"
  
  openapi_documents {
    document {
      path     = "spec.yaml"
      contents = base64encode(google_endpoints_service.pizza_api.openapi_config)
    }
  }
  
  gateway_config {
    backend_config {
      google_service_account = "${data.google_project.current.project_id}@appspot.gserviceaccount.com"
    }
  }
  
  lifecycle {
    create_before_destroy = true
  }
}

resource "google_api_gateway_gateway" "gateway" {
  provider = google
  
  api_config   = google_api_gateway_api_config.api_config.id
  gateway_id   = "pizza-api-gateway-${var.env.short}"
  display_name = "Pizza API Gateway"
  region       = data.google_client_config.current.region
}