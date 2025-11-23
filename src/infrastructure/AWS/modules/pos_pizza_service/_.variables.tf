variable "project" {
  type = object({
    short = string
    long  = string
  })
  description = <<EOF
    project = {
      short = "Short name of the project."
      long  = "Long name of the project."
    }
EOF
}

variable "env" {
  type = object({
    short = string
    long  = string
  })
  description = <<EOF
    env = {
      short = "Short name of the environment. Eg. `d`, `t`, `s`, `p`"
      long  = "Long name of the environment. Eg. `dev`, `test`, `stage`, `prod`"
    }
EOF
}

variable "docker_image" {
  type = object({
    image_name = string
    
  })
  description = <<EOF
    docker_image = {
      image_name = "Name of the docker image to use. default `pos-pizza-service`"
    }
EOF
  default = {
    image_name = "pos-pizza-service"
  }
}

variable "service_version" {
  type        = string
  description = "Version of the service. Eg. `0.1.0-alpha-107`"
}

variable "docker_build_date" {
  type        = string
  description = "Date and time when the docker file is build."
}

variable "docker_build_git_sha" {
  type        = string
  description = "GIT Sha of the build commit"
}

variable "cors_allowed_origins" {
  type        = list(string)
  description = "List of allowed origins for CORS. Use ['*'] to allow all origins."
  default     = ["*"]
}
