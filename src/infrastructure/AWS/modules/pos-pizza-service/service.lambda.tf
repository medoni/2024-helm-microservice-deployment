
resource "aws_lambda_function" "pos-pizza-service" {
  function_name = "pos-dev-pizza-service"
  role          = aws_iam_role.pos-pizza-service-lambda-exec-role.arn
  handler       = "PizzaService.Aws"
  runtime       = "dotnet8"
  filename      = local.pos-pizza-service-zip-file
  timeout       = 25

  depends_on = [null_resource.pos-pizza-service-container-image-operations]

  environment {
    variables = {
      "Logging__LogLevel__Default" = "Information"
      "Swagger__Enabled" = "True"
      "Aws__DynamoDb__Region" = data.aws_region.current.name
      "Aws__DynamoDb__MenusTableName" = aws_dynamodb_table.pos_pizza_service_menus_table.name
      "Aws__DynamoDb__CartsTableName" = aws_dynamodb_table.pos_pizza_service_carts_table.name
      "Aws__DynamoDb__OrdersTableName" = aws_dynamodb_table.pos_pizza_service_orders_table.name
    }
  }

  layers = [
    "arn:aws:lambda:eu-central-1:580247275435:layer:LambdaInsightsExtension:53"
  ]
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
