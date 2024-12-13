terraform {
  required_providers {
    local = {}
    aws = ">= 5.80.0"
    docker = {
      source  = "kreuzwerker/docker"
      version = ">= 3.0.2"
    }
  }
}
