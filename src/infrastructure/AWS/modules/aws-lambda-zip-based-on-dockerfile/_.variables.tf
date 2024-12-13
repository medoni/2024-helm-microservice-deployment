variable "function_name" {
  type = string
  description = "Name of the AWS Lambda function, used for naming related resources like IAM roles and policies."
}

variable "lambda" {
  type = object({
    handler     = string
    runtime     = string
    timeout     = number
    memory_size = number
  })
  description = <<EOF
    lambda = {
      handler     = "The entry point function to execute, e.g., 'index.handler' for Node.js."
      runtime     = "The runtime environment for the Lambda function, e.g., 'nodejs14.x', 'python3.9'."
      timeout     = "The maximum execution time (in seconds) for the function before it times out."
      memory_size = "The amount of memory (in MB) allocated to the function."
    }
EOF
}

variable "environment" {
  type = object({
    variables = map(string)
  })
  description = <<EOF
    environment = {
      variables = "A map of key-value pairs for environment variables to be passed to the Lambda function."
    }
EOF
}

variable "docker" {
  type = object({
    image_name    = string
    build = object({
      dockerfile  = string
      context     = string
      build_arg   = map(string)
    })
    image_app_dir = string
  })
  description = <<EOF
    docker = {
      image_name    = "The name of the Docker image to use for the Lambda function."
      build         = {
        dockerfile  = "Path to the Dockerfile used for building the image."
        context     = "The build context directory for the Docker image."
        build_arg   = "A map of build arguments passed to Docker during the build process."
      }
      image_app_dir = "The directory in the Docker image where the application resides."
    }
EOF
}

variable "layers" {
  type = list(string)
  description = "List of ARNs for AWS Lambda layers to attach to the function. Leave empty if no layers are needed."
  default = []
}

variable "cloudwatch" {
  type = object({
    retention_in_days = number
  })
  description = <<EOF
    cloudwatch = {
      retention_in_days = "CloudWatch log group retention policy in days."
    }
EOF
  default = {
    retention_in_days = 14
  }
}
