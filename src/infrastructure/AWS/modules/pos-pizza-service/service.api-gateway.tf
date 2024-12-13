resource "aws_api_gateway_rest_api" "pos_pizza_service_rest_api" {
  name          = "pos-dev-pizza-service"
}

# resource "aws_api_gateway_resource" "pos_pizza_service_api_resource" {
#   rest_api_id = aws_api_gateway_rest_api.pos_pizza_service_rest_api.id
#   parent_id   = aws_api_gateway_rest_api.pos_pizza_service_rest_api.root_resource_id
#   path_part   = "api"
# }

resource "aws_api_gateway_resource" "pos_pizza_service_proxy_service" {
  rest_api_id = aws_api_gateway_rest_api.pos_pizza_service_rest_api.id
  parent_id   = aws_api_gateway_rest_api.pos_pizza_service_rest_api.root_resource_id
  path_part   = "{proxy+}"
}

resource "aws_api_gateway_method" "pos_pizza_service_proxy_service" {
  rest_api_id   = aws_api_gateway_rest_api.pos_pizza_service_rest_api.id
  resource_id   = aws_api_gateway_resource.pos_pizza_service_proxy_service.id
  http_method   = "ANY"
  authorization = "NONE"
}

resource "aws_api_gateway_integration" "pos_pizza_service_lambda_integration" {
  rest_api_id             = aws_api_gateway_rest_api.pos_pizza_service_rest_api.id
  resource_id             = aws_api_gateway_resource.pos_pizza_service_proxy_service.id
  http_method             = aws_api_gateway_method.pos_pizza_service_proxy_service.http_method
  integration_http_method = "POST"
  type                    = "AWS_PROXY"
  uri                     = module.pos_pizza_service.lambda_invoke_arn
}

resource "aws_api_gateway_deployment" "pos_pizza_service_deployment" {
  rest_api_id = aws_api_gateway_rest_api.pos_pizza_service_rest_api.id
  depends_on = [
    aws_api_gateway_integration.pos_pizza_service_lambda_integration
  ]
}

resource "aws_api_gateway_stage" "pos_pizza_service_prod_stage" {
  deployment_id = aws_api_gateway_deployment.pos_pizza_service_deployment.id
  rest_api_id   = aws_api_gateway_rest_api.pos_pizza_service_rest_api.id
  stage_name    = "api"

  xray_tracing_enabled = true
}

resource "aws_lambda_permission" "api_gateway_invoke" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = module.pos_pizza_service.lambda_function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_api_gateway_rest_api.pos_pizza_service_rest_api.execution_arn}/*"
}
