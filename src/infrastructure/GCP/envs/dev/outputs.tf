output "pizza_service_url" {
  description = "The URL of the Pizza Service Cloud Run deployment"
  value       = module.pizza_service.pizza_service_url
}

output "firestore_database_name" {
  description = "The name of the Firestore database"
  value       = module.pizza_service.firestore_database_name
}

output "pubsub_order_created_topic" {
  description = "The Pub/Sub topic for order created events"
  value       = module.pizza_service.pubsub_order_created_topic
}

output "pubsub_payment_processed_topic" {
  description = "The Pub/Sub topic for payment processed events"
  value       = module.pizza_service.pubsub_payment_processed_topic
}