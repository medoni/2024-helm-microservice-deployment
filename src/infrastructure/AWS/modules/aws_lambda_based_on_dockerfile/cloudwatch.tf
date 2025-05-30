resource "aws_cloudwatch_log_group" "lambda_log_group" {
  name              = "/aws/lambda/${aws_lambda_function.aws_lambda_function.function_name}"
  retention_in_days = var.cloudwatch.retention_in_days
}
