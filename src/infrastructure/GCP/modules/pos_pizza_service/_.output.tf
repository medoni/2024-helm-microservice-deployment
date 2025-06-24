output "pizza_service_url" {
  description = "The URL of the Pizza Service Cloud Run deployment"
  value       = module.pizza_service.service_url
}

output "firestore_database_name" {
  description = "The name of the Firestore database"
  value       = google_firestore_database.database.name
}

output "pubsub_order_created_topic" {
  description = "The Pub/Sub topic for order created events"
  value       = google_pubsub_topic.order_created.name
}

output "pubsub_payment_processed_topic" {
  description = "The Pub/Sub topic for payment processed events"
  value       = google_pubsub_topic.payment_processed.name
}