resource "aws_apigatewayv2_api" "pos-pizza-service-hello-world-http_api" {
  name          = "hello-world-api"
  protocol_type = "HTTP"
}

resource "aws_apigatewayv2_integration" "pos-pizza-service-hello-world-lambda_integration" {
  api_id                 = aws_apigatewayv2_api.pos-pizza-service-hello-world-http_api.id
  integration_type       = "AWS_PROXY"
  integration_uri        = aws_lambda_function.pos-pizza-service.invoke_arn
  payload_format_version = "2.0"
}

resource "aws_apigatewayv2_route" "pos-pizza-service-hello-world-api_route" {
  api_id    = aws_apigatewayv2_api.pos-pizza-service-hello-world-http_api.id
  route_key = "ANY /api/{proxy+}"
  target    = "integrations/${aws_apigatewayv2_integration.pos-pizza-service-hello-world-lambda_integration.id}"
}

resource "aws_apigatewayv2_stage" "pos-pizza-service-hello-world-default-stage" {
  api_id      = aws_apigatewayv2_api.pos-pizza-service-hello-world-http_api.id
  name        = "$default"

  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.pos-pizza-service-hello-world-api-gw-access-logs.arn
    format          = jsonencode({
      requestId          = "$context.requestId",
      ip                 = "$context.identity.sourceIp",
      caller             = "$context.identity.caller",
      user               = "$context.identity.user",
      requestTime        = "$context.requestTime",
      httpMethod         = "$context.httpMethod",
      resourcePath       = "$context.resourcePath",
      status             = "$context.status",
      protocol           = "$context.protocol",
      responseLength     = "$context.responseLength"
    })
  }

  auto_deploy = true
}

resource "aws_cloudwatch_log_group" "pos-pizza-service-hello-world-api-gw-access-logs" {
  name              = "/aws/pos-dev-pizza-service-hello-world/access-logs"
  retention_in_days = 7
}

resource "aws_iam_role" "pos-pizza-service-hello-world-api-gw-logging-role" {
  name = "pos-dev-pizza-service-api-gateway-access-logging-policy"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action    = "sts:AssumeRole",
        Effect    = "Allow",
        Principal = {
          Service = "apigateway.amazonaws.com"
        },
      },
    ],
  })
}

resource "aws_iam_role_policy" "pos-pizza-service-hello-world-api-gw-logging-role-policy" {
  role = aws_iam_role.pos-pizza-service-hello-world-api-gw-logging-role.id

  policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action = [
          "logs:CreateLogStream",
          "logs:PutLogEvents"
        ],
        Effect   = "Allow",
        Resource = "${aws_cloudwatch_log_group.pos-pizza-service-hello-world-api-gw-access-logs.arn}:*"
      },
    ],
  })
}

resource "aws_lambda_permission" "api_gateway_invoke" {
  statement_id  = "AllowAPIGatewayInvoke"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.pos-pizza-service.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.pos-pizza-service-hello-world-http_api.execution_arn}/*"
}
