## Requirements

| Name | Version |
|------|---------|
| <a name="requirement_aws"></a> [aws](#requirement\_aws) | >= 5.80.0 |
| <a name="requirement_docker"></a> [docker](#requirement\_docker) | >= 3.0.2 |

## Providers

| Name | Version |
|------|---------|
| <a name="provider_aws"></a> [aws](#provider\_aws) | >= 5.80.0 |
| <a name="provider_docker"></a> [docker](#provider\_docker) | >= 3.0.2 |
| <a name="provider_null"></a> [null](#provider\_null) | n/a |

## Modules

No modules.

## Resources

| Name | Type |
|------|------|
| [aws_cloudwatch_log_group.lambda_log_group](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/cloudwatch_log_group) | resource |
| [aws_iam_role.lambda_exec_role](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/iam_role) | resource |
| [aws_iam_role_policy.lambda_role_cloudwatch_policy](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/iam_role_policy) | resource |
| [aws_lambda_function.aws_lambda_function](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/resources/lambda_function) | resource |
| [docker_image.docker_image](https://registry.terraform.io/providers/kreuzwerker/docker/latest/docs/resources/image) | resource |
| [null_resource.container_image_operations](https://registry.terraform.io/providers/hashicorp/null/latest/docs/resources/resource) | resource |
| [aws_caller_identity.current](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/data-sources/caller_identity) | data source |
| [aws_region.current](https://registry.terraform.io/providers/hashicorp/aws/latest/docs/data-sources/region) | data source |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_cloudwatch"></a> [cloudwatch](#input\_cloudwatch) | cloudwatch = {<br/>      retention\_in\_days = "CloudWatch log group retention policy in days."<br/>    } | <pre>object({<br/>    retention_in_days = number<br/>  })</pre> | <pre>{<br/>  "retention_in_days": 14<br/>}</pre> | no |
| <a name="input_docker"></a> [docker](#input\_docker) | docker = {<br/>      image\_name    = "The name of the Docker image to use for the Lambda function."<br/>      build         = {<br/>        dockerfile  = "Path to the Dockerfile used for building the image."<br/>        context     = "The build context directory for the Docker image."<br/>        build\_arg   = "A map of build arguments passed to Docker during the build process."<br/>      }<br/>      image\_app\_dir = "The directory in the Docker image where the application resides."<br/>    } | <pre>object({<br/>    image_name    = string<br/>    build = object({<br/>      dockerfile  = string<br/>      context     = string<br/>      build_arg   = map(string)<br/>    })<br/>    image_app_dir = string<br/>  })</pre> | n/a | yes |
| <a name="input_environment"></a> [environment](#input\_environment) | environment = {<br/>      variables = "A map of key-value pairs for environment variables to be passed to the Lambda function."<br/>    } | <pre>object({<br/>    variables = map(string)<br/>  })</pre> | n/a | yes |
| <a name="input_function_name"></a> [function\_name](#input\_function\_name) | Name of the AWS Lambda function, used for naming related resources like IAM roles and policies. | `string` | n/a | yes |
| <a name="input_lambda"></a> [lambda](#input\_lambda) | lambda = {<br/>      handler     = "The entry point function to execute, e.g., 'index.handler' for Node.js."<br/>      runtime     = "The runtime environment for the Lambda function, e.g., 'nodejs14.x', 'python3.9'."<br/>      timeout     = "The maximum execution time (in seconds) for the function before it times out."<br/>      memory\_size = "The amount of memory (in MB) allocated to the function."<br/>    } | <pre>object({<br/>    handler     = string<br/>    runtime     = string<br/>    timeout     = number<br/>    memory_size = number<br/>  })</pre> | n/a | yes |
| <a name="input_layers"></a> [layers](#input\_layers) | List of ARNs for AWS Lambda layers to attach to the function. Leave empty if no layers are needed. | `list(string)` | `[]` | no |

## Outputs

| Name | Description |
|------|-------------|
| <a name="output_iam_role_exec_role"></a> [iam\_role\_exec\_role](#output\_iam\_role\_exec\_role) | The name of the IAM execution role associated with the Lambda function. |
| <a name="output_iam_role_exec_role_arn"></a> [iam\_role\_exec\_role\_arn](#output\_iam\_role\_exec\_role\_arn) | The ARN of the IAM role assumed by the AWS Lambda function for execution. |
| <a name="output_lambda_function_name"></a> [lambda\_function\_name](#output\_lambda\_function\_name) | The name of the AWS Lambda function. |
| <a name="output_lambda_invoke_arn"></a> [lambda\_invoke\_arn](#output\_lambda\_invoke\_arn) | The ARN to invoke the AWS Lambda function. |
