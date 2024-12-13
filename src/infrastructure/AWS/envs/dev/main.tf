
module "aws_account" {
  source = "../../modules/aws_account"
}

module "pos_pizza_service" {
    source = "../../modules/pos_pizza_service"

    project = var.project
    env     = var.env

    service_version      = "0.1.0-alpha-108"
    docker_build_date    = "1970-01-01T00:00:00Z"
    docker_build_git_sha = "A100000000000000000000000000000000000000"
}
