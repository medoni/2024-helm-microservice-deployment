resource "aws_lambda_function" "aws_lambda_function" {
  function_name  = var.function_name
  role           = aws_iam_role.lambda_exec_role.arn
  timeout        = var.lambda.timeout
  memory_size    = var.lambda.memory_size

  image_uri    = module.docker_image.image_uri
  package_type = "Image"

  tracing_config {
    mode = "Active"
  }

  environment {
    variables = var.environment.variables
  }
}
