resource "docker_image" "docker_image" {
  name = var.docker.image_name
  build {
    context    = var.docker.build.context
    dockerfile = var.docker.build.dockerfile
    build_arg  = var.docker.build.build_arg 
  }
}
