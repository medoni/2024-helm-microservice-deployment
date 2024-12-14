output "lambda_invoke_arn" {
  value       = aws_lambda_function.aws_lambda_function.invoke_arn
  description = "The ARN to invoke the AWS Lambda function."
}

output "lambda_function_name" {
  value       = aws_lambda_function.aws_lambda_function.function_name
  description = "The name of the AWS Lambda function."
}

output "iam_role_exec_role_arn" {
  value       = aws_iam_role.lambda_exec_role.arn
  description = "The ARN of the IAM role assumed by the AWS Lambda function for execution."
}

output "iam_role_exec_role" {
  value       = aws_iam_role.lambda_exec_role.name
  description = "The name of the IAM execution role associated with the Lambda function."
}
