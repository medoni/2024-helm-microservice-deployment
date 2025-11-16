output "api_gateway_url" {
  description = "The URL of the Pizza Service API Gateway"
  value       = module.pos_pizza_service.api_gateway_invoke_url
}

output "frontend_url" {
  description = "The URL to access the frontend application"
  value       = module.frontend_app.frontend_url
}

output "frontend_cloudfront_distribution_id" {
  description = "The CloudFront distribution ID for the frontend"
  value       = module.frontend_app.cloudfront_distribution_id
}

output "frontend_s3_bucket" {
  description = "The S3 bucket name hosting the frontend"
  value       = module.frontend_app.s3_bucket_name
}
