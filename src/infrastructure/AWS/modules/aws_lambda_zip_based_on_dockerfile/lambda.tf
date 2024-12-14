resource "aws_lambda_function" "aws_lambda_function" {
  function_name = var.function_name
  role          = aws_iam_role.lambda_exec_role.arn
  handler       = var.lambda.handler
  runtime       = var.lambda.runtime
  filename      = local.tmp_docker_image_zip
  timeout       = var.lambda.timeout
  memory_size   = var.lambda.memory_size

  depends_on = [null_resource.container_image_operations]

  lifecycle {
    replace_triggered_by = [
      docker_image.docker_image.id
    ]
  }

  environment {
    variables = var.environment.variables
  }

  layers = var.layers
}
