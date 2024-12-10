variable service-version {
  type        = string
  default     = "0.1.0-alpha-101"
  description = "description"
}

resource "docker_image" "pos-pizza-service" {
  name = "pos-dev-pizza-service"
  build {
    context    = "../../../../"
    dockerfile = "backend/Deployables/PizzaService.Aws/Dockerfile"
    build_arg = {
      BUILD_VERSION : var.service-version
      BUILD_DATE: "1970-01-01T00:00:00Z"
      GIT_SHA: "A100000000000000000000000000000000000000"
    }
  }
}

locals {
  pos-pizza-service-zip-file = "${path.module}/.tmp/pizza-service-v${var.service-version}.zip"
}

resource "null_resource" "pos-pizza-service-container-image-operations" {
  provisioner "local-exec" {
    interpreter = ["pwsh", "-Command"]

    command = <<EOF
$containerId = (docker create ${docker_image.pos-pizza-service.image_id}).Trim()
$outputDir = "${path.module}/.tmp/pizza-service-app-content"
if (-Not (Test-Path $outputDir)) { New-Item -ItemType Directory -Path $outputDir }

docker cp "$${containerId}:/app" $outputDir
Compress-Archive -Path "$outputDir/app/*" -DestinationPath "${local.pos-pizza-service-zip-file}" -CompressionLevel Optimal
docker rm $containerId

EOF
  }

  depends_on = [ docker_image.pos-pizza-service ]

  lifecycle {
    replace_triggered_by = [
      docker_image.pos-pizza-service.id
    ]
  }
}
