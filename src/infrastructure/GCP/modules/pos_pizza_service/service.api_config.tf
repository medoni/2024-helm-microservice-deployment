locals {
  swagger_content = file("${path.module}/swagger.yaml")
  swagger_hash = filemd5("${path.module}/swagger.yaml")

  swagger_replaced_content = replace(local.swagger_content, "$${TARGET_URL}", module.pizza_service.service_url)
}

resource "google_api_gateway_api_config" "pizza_api_config" {
  provider      = google-beta
  api           = google_api_gateway_api.pizza_api.api_id
  api_config_id = "${var.project.short}-${var.env.short}-${local.swagger_hash}-config"
  display_name  = "Pizza API Config"
  
  openapi_documents {
    document {
      path = "swagger.yaml"
      contents = base64encode(local.swagger_replaced_content)
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
