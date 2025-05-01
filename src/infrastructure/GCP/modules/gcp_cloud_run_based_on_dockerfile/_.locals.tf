locals {
  docker_image_name = "${var.docker.image_name}:${var.docker.image_tag}"
  gcr_image_name    = "${data.google_project.current.project_id}/${var.docker.image_name}:${var.docker.image_tag}"
  gcr_location      = "${data.google_client_config.current.region}-docker.pkg.dev"
  image_uri         = "${local.gcr_location}/${local.gcr_image_name}"
}