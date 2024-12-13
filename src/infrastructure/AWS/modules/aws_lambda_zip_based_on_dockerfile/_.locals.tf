locals {
  docker_image_id          = split(":", docker_image.docker_image.image_id)[1]
  tmp_docker_image_app_dir = "${path.module}/.tmp/${local.docker_image_id}"
  tmp_docker_image_zip     = "${path.module}/.tmp/${local.docker_image_id}.zip"
}
