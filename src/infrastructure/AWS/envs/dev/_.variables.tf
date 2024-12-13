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
