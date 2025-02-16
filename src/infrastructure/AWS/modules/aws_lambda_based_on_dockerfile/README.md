# Terraform Module: Lambda Deployment with Dockerfile to AWS
This Terraform module simplifies the deployment of an AWS Lambda function based on a Dockerfile. It provisions the necessary resources, including:

- The Lambda function itself
- AWS ECR
- Required IAM policies
- CloudWatch integration for monitoring and logging

## Usage

```terraform
module "my_service" {
  source = "../aws_lambda_based_on_dockerfile"
  
  function_name = "${var.project.short}-${var.env.short}-my-service-name"

  lambda = {
    timeout       = 25
    memory_size   = 256
  }

  docker = {
    image_name = var.docker_image.image_name
    image_tag  = var.service_version
    build = {
      dockerfile = "backend/Deployables/MyService/Dockerfile"
      context = "${path.cwd}/../../../../"
      build_arg = {
        BUILD_VERSION = var.service_version
        BUILD_DATE    = var.docker_build_date
        GIT_SHA       = var.docker_build_git_sha
      }
    }
    image_app_dir = "/app"
  }

  environment = {
    variables = {
      "Logging__LogLevel__Default" = "Information"
      "Swagger__Enabled" = "True"
      "Aws__DynamoDb__Region" = data.aws_region.current.name
      "Aws__DynamoDb__MenusTableName" = aws_dynamodb_table.my_service_table.name
    }
  }

  cloudwatch = {
    retention_in_days = 7
  }
}
```

The Dockerfile:
```Dockerfile
# build context /src

ARG BUILD_VERSION="0.1.0-alpha-1"
ARG BUILD_DATE="1970-01-01T00:00:00Z"
ARG GIT_SHA="0000000000000000000000000000000000000000"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    ARG BUILD_VERSION
    ARG GIT_SHA

    WORKDIR /src
    COPY . .
    RUN dotnet publish backend/Deployables/MyService/*.csproj \
        -c Release \
        --self-contained false \
        -r linux-x64 \
        -o /app/publish \
        /p:MINVERVERSIONOVERRIDE=$BUILD_VERSION \
        /p:SourceRevisionId=$GIT_SHA

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    ARG BUILD_VERSION
    ARG BUILD_DATE
    ARG GIT_SHA

    WORKDIR /app
    COPY --from=build /app/publish .

    ENTRYPOINT dotnet PizzaService.Aws.dll

```

<!-- BEGIN_TF_DOCS -->
## Modules

| Name | Source | Version |
|------|--------|---------|
| <a name="module_docker_image"></a> [docker\_image](#module\_docker\_image) | terraform-aws-modules/lambda/aws//modules/docker-build | n/a |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_docker"></a> [docker](#input\_docker) | docker = {<br/>      image\_name    = "The name of the Docker image to use for the Lambda function."<br/>      image\_tag     = "The name of Docker image tag."<br/>      build         = {<br/>        dockerfile  = "Path to the Dockerfile used for building the image."<br/>        context     = "The build context directory for the Docker image."<br/>        build\_arg   = "A map of build arguments passed to Docker during the build process."<br/>      }<br/>      image\_app\_dir = "The directory in the Docker image where the application resides."<br/>    } | <pre>object({<br/>    image_name    = string<br/>    image_tag     = string<br/>    build = object({<br/>      dockerfile  = string<br/>      context     = string<br/>      build_arg   = map(string)<br/>    })<br/>    image_app_dir = string<br/>  })</pre> | n/a | yes |
| <a name="input_environment"></a> [environment](#input\_environment) | environment = {<br/>      variables = "A map of key-value pairs for environment variables to be passed to the Lambda function."<br/>    } | <pre>object({<br/>    variables = map(string)<br/>  })</pre> | n/a | yes |
| <a name="input_function_name"></a> [function\_name](#input\_function\_name) | Name of the AWS Lambda function, used for naming related resources like IAM roles and policies. | `string` | n/a | yes |
| <a name="input_lambda"></a> [lambda](#input\_lambda) | lambda = {<br/>      handler     = "The entry point function to execute, e.g., 'index.handler' for Node.js."<br/>      runtime     = "The runtime environment for the Lambda function, e.g., 'nodejs14.x', 'python3.9'."<br/>      timeout     = "The maximum execution time (in seconds) for the function before it times out."<br/>      memory\_size = "The amount of memory (in MB) allocated to the function."<br/>    } | <pre>object({<br/>    timeout     = number<br/>    memory_size = number<br/>  })</pre> | n/a | yes |
| <a name="input_cloudwatch"></a> [cloudwatch](#input\_cloudwatch) | cloudwatch = {<br/>      retention\_in\_days = "CloudWatch log group retention policy in days."<br/>    } | <pre>object({<br/>    retention_in_days = number<br/>  })</pre> | <pre>{<br/>  "retention_in_days": 14<br/>}</pre> | no |

## Outputs

| Name | Description |
|------|-------------|
| <a name="output_iam_role_exec_role"></a> [iam\_role\_exec\_role](#output\_iam\_role\_exec\_role) | The name of the IAM execution role associated with the Lambda function. |
| <a name="output_iam_role_exec_role_arn"></a> [iam\_role\_exec\_role\_arn](#output\_iam\_role\_exec\_role\_arn) | The ARN of the IAM role assumed by the AWS Lambda function for execution. |
| <a name="output_lambda_function_name"></a> [lambda\_function\_name](#output\_lambda\_function\_name) | The name of the AWS Lambda function. |
| <a name="output_lambda_invoke_arn"></a> [lambda\_invoke\_arn](#output\_lambda\_invoke\_arn) | The ARN to invoke the AWS Lambda function. |
| <a name="output_lambda_log_group_name"></a> [lambda\_log\_group\_name](#output\_lambda\_log\_group\_name) | The name of the log group where the lambda logs to. |
<!-- END_TF_DOCS -->
