resource "null_resource" "frontend_build" {
  triggers = {
    # Rebuild when configuration changes
    config_pizza_api_url = var.config.pizza_api_url
    version = var.app_version
  }

  provisioner "local-exec" {
    command     = "${path.module}/scripts/build-and-upload.sh"
    working_dir = path.module

    environment = {
      FRONTEND_SOURCE_PATH  = var.frontend.source_path
      FRONTEND_BUILD_PATH   = var.frontend.build_path
      CONFIG_PIZZA_API_URL  = var.config.pizza_api_url
      S3_BUCKET_NAME        = aws_s3_bucket.frontend_app.id
      CLOUDFRONT_DIST_ID    = aws_cloudfront_distribution.frontend_app.id
    }
  }

  depends_on = [
    aws_s3_bucket.frontend_app,
    aws_cloudfront_distribution.frontend_app
  ]
}
