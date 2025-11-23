
module "aws_account" {
  source = "../../modules/aws_account"
}

module "pos_pizza_service" {
    source = "../../modules/pos_pizza_service"

    project = var.project
    env     = var.env

    service_version      = var.service_version
    docker_build_date    = var.build_date
    docker_build_git_sha = var.build_git_sha

    # CORS: Allow all origins for development
    # For production, restrict to specific frontend domain after deployment
    cors_allowed_origins = ["*"]
}

module "frontend_app" {
  source = "../../modules/frontend_app"

  project = var.project
  env     = var.env

  frontend = {
    source_path = "${path.cwd}/../../../../frontend"
    build_path  = "build"
  }

  config = {
    pizza_api_url = module.pos_pizza_service.api_gateway_invoke_url
  }

  app_version = var.service_version
}
