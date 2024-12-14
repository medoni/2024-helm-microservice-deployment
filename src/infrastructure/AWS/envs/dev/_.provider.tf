provider "aws" {
  region = "eu-central-1"
  default_tags {
   tags = {
     Environment = "dev"
     Owner       = "https://github.com/medoni"
     Project     = "pos-pizza-service"
   }
 }
}

# provider "docker" {
#   registry_auth {
#     address  = "715841335307.dkr.ecr.eu-central-1.amazonaws.com"
#   }
# }

terraform {
  required_providers {
    aws = "~> 5.80.0"
    docker = {
      source  = "kreuzwerker/docker"
      version = "3.0.2"
    }
  }

  backend "s3" {
    bucket         = "pos-dev-terraform"
    key            = "terraform/state/pos-dev.tfstate"
    region         = "eu-central-1"
    dynamodb_table = "pos-dev-terraform-state-lock"
    encrypt        = true
  }
}
