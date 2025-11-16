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

variable "frontend" {
  type = object({
    source_path = string
    build_path  = string
  })
  description = <<EOF
    frontend = {
      source_path = "Path to the frontend source code directory."
      build_path  = "Path to the built frontend assets (usually 'build' or 'dist')."
    }
EOF
}

variable "config" {
  type = object({
    pizza_api_url = string
  })
  description = <<EOF
    config = {
      pizza_api_url = "URL of the Pizza API for the frontend configuration."
    }
EOF
}

variable "domain_name" {
  type        = string
  description = "Optional domain name for CloudFront distribution (e.g., 'app.example.com'). Leave empty for CloudFront default domain."
  default     = ""
}

variable "route53_zone_id" {
  type        = string
  description = "Optional Route53 hosted zone ID for creating DNS records. Required if domain_name is set."
  default     = ""
}

variable "acm_certificate_arn" {
  type        = string
  description = "Optional ACM certificate ARN for HTTPS. Required if domain_name is set. Must be in us-east-1 region."
  default     = ""
}

variable "app_version" {
  type        = string
  description = "Version of the app. Eg. `0.1.0-alpha-107`"
}