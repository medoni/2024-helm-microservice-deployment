resource "google_logging_metric" "service_error_metric" {
  name        = "${var.service_name}-error-count"
  description = "Error count for the ${var.service_name} service"
  filter      = "resource.type=\"cloud_run_revision\" AND resource.labels.service_name=\"${var.service_name}\" AND severity>=ERROR"
  metric_descriptor {
    metric_kind = "DELTA"
    value_type  = "INT64"
    labels {
      key         = "service"
      value_type  = "STRING"
      description = "Service name"
    }
  }
  label_extractors = {
    "service" = "EXTRACT(resource.labels.service_name)"
  }
}

resource "google_logging_project_bucket_config" "logging_bucket" {
  project        = data.google_project.current.project_id
  location       = data.google_client_config.current.region
  bucket_id      = "${var.service_name}-logs"
  retention_days = var.logging.retention_in_days
}