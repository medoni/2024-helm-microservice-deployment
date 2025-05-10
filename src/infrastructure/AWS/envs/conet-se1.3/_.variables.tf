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
  default = {
    short = "pos"
    long  = "Pizza-Ordering-Service"
  }
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
  default = {
    short = "d"
    long  = "dev"
  }
}

variable "service_version" {
  type        = string
  description = "Version of the service."
  default     = "0.1.0-alpha-119"
}

variable "build_date" {
  type        = string
  description = "Date and time when the service is beeing build."
  default     = "1970-01-01T00:00:00Z"
}

variable "build_git_sha" {
  type        = string
  description = "GIT SHA of the commit that is build."
  default     = "A100000000000000000000000000000000000000"
}
