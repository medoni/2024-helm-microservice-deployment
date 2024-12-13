resource "null_resource" "container_image_operations" {
  provisioner "local-exec" {
    interpreter = ["pwsh", "-Command"]

    command = <<EOF
$containerId = (docker create ${docker_image.docker_image.image_id}).Trim()
$outputDir = "${local.tmp_docker_image_app_dir}"
if (-Not (Test-Path $outputDir)) { New-Item -ItemType Directory -Path $outputDir }

docker cp "$${containerId}:${var.docker.image_app_dir}/" $outputDir
Compress-Archive -Force -Path "$outputDir${var.docker.image_app_dir}/*" -DestinationPath "${local.tmp_docker_image_zip}" -CompressionLevel Optimal
docker rm $containerId

EOF
  }

  depends_on = [ docker_image.docker_image ]

  lifecycle {
    replace_triggered_by = [
      docker_image.docker_image.id
    ]
  }
}
