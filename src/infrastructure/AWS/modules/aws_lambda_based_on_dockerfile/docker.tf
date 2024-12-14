module "docker_image" {
  source = "terraform-aws-modules/lambda/aws//modules/docker-build"

  create_ecr_repo = true
  ecr_repo        = "${var.function_name}-repo"

  use_image_tag = true
  image_tag     = var.docker.image_tag

  source_path      = var.docker.build.context
  docker_file_path = var.docker.build.dockerfile
  build_args       = var.docker.build.build_arg
}
