
module "pos_pizza_service" {
  source = "../aws_lambda_zip_based_on_dockerfile"

  function_name = "${var.project.short}-${var.env.short}-pizza-service"

  lambda = {
    handler       = "PizzaService.Aws"
    runtime       = "dotnet8"
    timeout       = 25
    memory_size   = 256
  }

  docker = {
    image_name = var.docker_image.image_name
    build = {
      dockerfile = "backend/Deployables/PizzaService.Aws/Dockerfile"
      context = "${path.cwd}/../../../../"
      build_arg = {
        BUILD_VERSION = var.service_version
        BUILD_DATE    = var.docker_build_date
        GIT_SHA       = var.docker_build_git_sha
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
