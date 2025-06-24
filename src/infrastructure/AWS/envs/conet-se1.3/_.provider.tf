provider "aws" {
  region = "eu-central-1"
  default_tags {
   tags = {
     Environment = "dev"
     Owner       = "meichhorn@conet.de"
     Project     = "pos-pizza-service"
   }
 }
}

terraform {
  required_providers {
    aws = "~> 5.80.0"
    docker = {
      source  = "kreuzwerker/docker"
      version = "3.0.2"
    }
  }

  backend "s3" {
    bucket         = "pos-se13-terraform"
    key            = "terraform/state/pos-se13.tfstate"
    region         = "eu-central-1"
    dynamodb_table = "pos-se13-terraform-state-lock"
    encrypt        = true
  }
}
