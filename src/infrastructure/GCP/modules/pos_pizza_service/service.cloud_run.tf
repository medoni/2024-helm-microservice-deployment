module "pizza_service" {
  source = "../gcp_cloud_run_based_on_dockerfile"
  
  service_name = "${var.project.short}-${var.env.short}-pizza-service"

  cloud_run = {
    timeout_seconds = 300
    memory          = "512Mi"
    cpu             = "1"
    max_instances   = 10
  }

  docker = {
    image_name = "pizza-service"
    image_tag  = var.service_version
    build = {
      dockerfile = "backend/Deployables/PizzaService.AspNetCore/Dockerfile"
      context    = "${path.root}/../../../../"
      build_arg  = {
        BUILD_VERSION = var.service_version
        BUILD_DATE    = var.build_date
        GIT_SHA       = var.build_git_sha
      }
    }
  }

  environment = {
    variables = {
      "Logging__LogLevel__Default" = "Information"
      "Swagger__Enabled"           = "True"
      "GOOGLE_CLOUD_PROJECT"       = data.google_project.current.project_id
      "Firestore__Database"        = google_firestore_database.database.name
      "Firestore__CollectionMenus" = "menus"
      "Firestore__CollectionCarts" = "carts"
      "Firestore__CollectionOrders" = "orders"
    }
  }

  logging = {
    retention_in_days = 14
  }
}