
variable service-version {
  type        = string
  default     = "0.1.0-alpha-107"
  description = "description"
}

module "pos_pizza_service" {
  source = "../aws-lambda-zip-based-on-dockerfile"

  function_name = "pos-dev-pizza-service"

  lambda = {
    handler       = "PizzaService.Aws"
    runtime       = "dotnet8"
    timeout       = 25
    memory_size   = 256
  }

  docker = {
    image_name = "pos-pizza-service"
    build = {
      dockerfile = "backend/Deployables/PizzaService.Aws/Dockerfile"
      context = "${path.cwd}/../../../../"
      build_arg = {
        BUILD_VERSION = var.service-version
        BUILD_DATE = "1970-01-01T00:00:00Z"
        GIT_SHA = "A100000000000000000000000000000000000000"
      }
    }
    image_app_dir = "/app"
  }

  environment = {
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

  cloudwatch = {
    retention_in_days = 7
  }
}
