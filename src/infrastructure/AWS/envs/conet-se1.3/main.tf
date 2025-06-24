
module "aws_account" {
  source = "../../modules/aws_account"

  iam_roles = {
    contributor = {
      enabled = true
      name    = "${var.project.short}-contributor"
    }
  }
}

module "pos_pizza_service" {
    source = "../../modules/pos_pizza_service"

    project = var.project
    env     = var.env

    service_version      = var.service_version
    docker_build_date    = var.build_date
    docker_build_git_sha = var.build_git_sha
}
