locals {
  tmp_docker_image_app_dir = "${path.module}/.tmp/${var.docker.image_name}"
  tmp_docker_image_zip     = "${path.module}/.tmp/${var.docker.image_name}.zip"
}
