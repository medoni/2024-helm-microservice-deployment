resource "google_firestore_database" "database" {
  name        = "${var.project.short}-${var.env.short}-db"
  location_id = data.google_client_config.current.region
  type        = "FIRESTORE_NATIVE"
  
  depends_on = [google_project_service.firestore_api]
}

resource "google_firestore_index" "menus_index" {
  collection  = "menus"
  database    = google_firestore_database.database.name
  
  fields {
    field_path = "category"
    order      = "ASCENDING"
  }
  
  fields {
    field_path = "name"
    order      = "ASCENDING"
  }
}

# Index for cart items
resource "google_firestore_index" "carts_index" {
  collection  = "carts"
  database    = google_firestore_database.database.name
  
  fields {
    field_path = "customerId"
    order      = "ASCENDING"
  }
  
  fields {
    field_path = "createdAt"
    order      = "DESCENDING"
  }
}

# Index for orders
resource "google_firestore_index" "orders_index" {
  collection  = "orders"
  database    = google_firestore_database.database.name
  
  fields {
    field_path = "customerId"
    order      = "ASCENDING"
  }
  
  fields {
    field_path = "orderDate"
    order      = "DESCENDING"
  }
}