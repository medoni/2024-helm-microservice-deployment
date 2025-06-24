variable "service_name" {
  type        = string
  description = "Name of the Google Cloud Run service, used for naming related resources like IAM roles and policies."
}

variable "cloud_run" {
  type = object({
    timeout_seconds = number
    memory          = string
    cpu             = string
    max_instances   = number
  })
  description = <<EOF
    cloud_run = {
      timeout_seconds = "The maximum execution time (in seconds) for the service before it times out."
      memory          = "The amount of memory allocated to the service (e.g., '256Mi', '1Gi')."
      cpu             = "The number of CPUs allocated to the service (e.g., '1', '2')."
      max_instances   = "The maximum number of container instances allowed to start."
    }
EOF
}

variable "environment" {
  type = object({
    variables = map(string)
  })
  description = <<EOF
    environment = {
      variables = "A map of key-value pairs for environment variables to be passed to the Cloud Run service."
    }
EOF
}

variable "docker" {
  type = object({
    image_name    = string
    image_tag     = string
    build = object({
      dockerfile  = string
      context     = string
      build_arg   = map(string)
    })
  })
  description = <<EOF
    docker = {
      image_name    = "The name of the Docker image to use for the Cloud Run service."
      image_tag     = "The name of Docker image tag."
      build         = {
        dockerfile  = "Path to the Dockerfile used for building the image."
        context     = "The build context directory for the Docker image."
        build_arg   = "A map of build arguments passed to Docker during the build process."
      }
    }
EOF
}

variable "logging" {
  type = object({
    retention_in_days = number
  })
  description = <<EOF
    logging = {
      retention_in_days = "Cloud Logging retention policy in days."
    }
EOF
  default = {
    retention_in_days = 14
  }
}