
resource "aws_lambda_function" "pos-pizza-service" {
  function_name = "pos-dev-pizza-service"
  role          = aws_iam_role.pos-pizza-service-lambda-exec-role.arn
  // handler       = "MyApp::Example.Hello::MyHandler"
  runtime       = "dotnet8"
  filename      = local.pos-pizza-service-zip-file #data.local_file.pos-pizza-service-lambda-zip.filename

  //source_code_hash = filebase64sha256(local.pos-pizza-service-zip-file)

  depends_on = [null_resource.pos-pizza-service-container-image-operations]
}

resource "aws_iam_role" "pos-pizza-service-lambda-exec-role" {
  name = "pos-dev-pizza-service-lambda-exec-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action    = "sts:AssumeRole"
        Effect    = "Allow"
        Principal = {
          Service = "lambda.amazonaws.com"
        }
      }
    ]
  })
}

resource "aws_iam_role_policy" "pos-pizza-service-lambda-role-policy" {
  name   = "pos-dev-pizza-service-lambda-role-policy"
  role   = aws_iam_role.pos-pizza-service-lambda-exec-role.id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action   = [
          "logs:CreateLogGroup",
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ]
        Effect   = "Allow"
        Resource = "arn:aws:logs:*:*:*"
      }
    ]
  })
}

resource "aws_cloudwatch_log_group" "pos-pizza-service-hello-world-lamda-" {
  name              = "/aws/lambda/${aws_lambda_function.pos-pizza-service.function_name}"
  retention_in_days = 7
}
