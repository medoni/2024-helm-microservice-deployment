output "cloudfront_distribution_id" {
  description = "The ID of the CloudFront distribution"
  value       = aws_cloudfront_distribution.frontend_app.id
}

output "cloudfront_distribution_arn" {
  description = "The ARN of the CloudFront distribution"
  value       = aws_cloudfront_distribution.frontend_app.arn
}

output "cloudfront_domain_name" {
  description = "The domain name of the CloudFront distribution"
  value       = aws_cloudfront_distribution.frontend_app.domain_name
}

output "s3_bucket_name" {
  description = "The name of the S3 bucket hosting the frontend"
  value       = aws_s3_bucket.frontend_app.id
}

output "s3_bucket_arn" {
  description = "The ARN of the S3 bucket hosting the frontend"
  value       = aws_s3_bucket.frontend_app.arn
}

output "frontend_url" {
  description = "The URL to access the frontend application"
  value       = local.use_custom_domain ? "https://${var.domain_name}" : "https://${aws_cloudfront_distribution.frontend_app.domain_name}"
}
