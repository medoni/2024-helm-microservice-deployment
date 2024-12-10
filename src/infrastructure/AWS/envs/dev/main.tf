
module "aws-account" {
  source = "../../modules/aws-account"
}

module "pos-pizza-service" {
    source = "../../modules/pos-pizza-service"
}
