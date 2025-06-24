locals {
  docker_image_name = "${var.docker.image_name}:${var.docker.image_tag}"
  gcr_repository    = "${data.google_client_config.current.region}-docker.pkg.dev/${data.google_project.current.project_id}/${var.docker.image_name}"
  image_uri         = "${local.gcr_repository}/${var.docker.image_name}:${var.docker.image_tag}"
}