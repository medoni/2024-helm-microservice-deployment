provider "google" {
  project = "pizza-ordering-service-458504"
  region  = "europe-west1"
}

provider "google-beta" {
  project = "pizza-ordering-service-458504"
  region  = "europe-west1"
}

provider "docker" {
  host = "unix:///var/run/docker.sock"
}

terraform {
  required_providers {
    google = {
      source  = "hashicorp/google"
      version = ">= 4.80.0"
    }
    google-beta = {
      source  = "hashicorp/google-beta"
      version = ">= 4.80.0"
    }
    docker = {
      source  = "kreuzwerker/docker"
      version = ">= 3.0.2"
    }
  }
  
  backend "gcs" {
    bucket = "pizza-ordering-service-458504-tfstate"
    prefix = "terraform/state"
  }
}

module "pizza_service" {
  source = "../../modules/pos_pizza_service"
  
  project = {
    short = "pos"
    long  = "Pizza Ordering System"
  }
  
  env = {
    short = "dev"
    long  = "Development"
  }
  
  service_version = "0.1.0-alpha-10"
  build_date      = "2025-05-01T00:00:00Z"
  build_git_sha   = "123456789abcdef"
}