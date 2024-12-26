resource "aws_ssm_parameter" "pizza_service_paypal_client_id" {
  name        = "/${var.project.short}/${var.env.long}/Pizza-Service/PaypalService/ClientId"
  description = "ClientID for the Paypal API"
  type        = "SecureString"
  value       = "MISSING"

  lifecycle {
    ignore_changes = [ value ]
  }
}

resource "aws_ssm_parameter" "pizza_service_paypal_client_secret" {
  name        = "/${var.project.short}/${var.env.long}/Pizza-Service/PaypalService/ClientSecret"
  description = "ClientSecret for the Paypal API"
  type        = "SecureString"
  value       = "MISSING"

  lifecycle {
    ignore_changes = [ value ]
  }
}
