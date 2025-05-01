resource "google_pubsub_topic" "order_created" {
  name = "${var.project.short}-${var.env.short}-order-created"
}

resource "google_pubsub_topic" "payment_processed" {
  name = "${var.project.short}-${var.env.short}-payment-processed"
}

resource "google_pubsub_subscription" "order_created_subscription" {
  name  = "${var.project.short}-${var.env.short}-order-created-sub"
  topic = google_pubsub_topic.order_created.name

  ack_deadline_seconds = 20

  retry_policy {
    minimum_backoff = "10s"
  }

  expiration_policy {
    ttl = "86400s" # 24 hours
  }
}

resource "google_pubsub_subscription" "payment_processed_subscription" {
  name  = "${var.project.short}-${var.env.short}-payment-processed-sub"
  topic = google_pubsub_topic.payment_processed.name

  ack_deadline_seconds = 20

  retry_policy {
    minimum_backoff = "10s"
  }

  expiration_policy {
    ttl = "86400s" # 24 hours
  }
}