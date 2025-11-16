output "api_gateway_invoke_url" {
  description = "The invoke URL of the API Gateway stage"
  value       = "${aws_api_gateway_stage.pos_pizza_service_prod_stage.invoke_url}"
}

output "api_gateway_rest_api_id" {
  description = "The ID of the API Gateway REST API"
  value       = aws_api_gateway_rest_api.pos_pizza_service_rest_api.id
}

output "api_gateway_stage_name" {
  description = "The name of the API Gateway stage"
  value       = aws_api_gateway_stage.pos_pizza_service_prod_stage.stage_name
}
