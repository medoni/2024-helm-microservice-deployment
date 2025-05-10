# Deployment dev Environment
This Terraform module deploys the Pizza-Ordering-System to the _CONET SE1.3_ environment hosted on AWS. 
## Usage

```powershell
$ $env:AWS_PROFILE="<profile>"
$ aws sso login
$ aws ecr get-login-password | docker login --username AWS --password-stdin 074682455637.dkr.ecr.eu-central-1.amazonaws.com/pos-d-pizza-service-repo

$ terraform init # optional
$ tf apply `
   -var 'service_version=0.0.1-alpha-3' `
   -var 'build_date=1970-01-01T00:00:00Z' `
   -var 'build_git_sha=A100000000000000000000000000000000000000'
```


## Requirements

| Name | Version |
|------|---------|
| <a name="requirement_aws"></a> [aws](#requirement\_aws) | ~> 5.80.0 |
| <a name="requirement_docker"></a> [docker](#requirement\_docker) | 3.0.2 |

## Providers

No providers.

## Modules

| Name | Source | Version |
|------|--------|---------|
| <a name="module_aws_account"></a> [aws\_account](#module\_aws\_account) | ../../modules/aws_account | n/a |
| <a name="module_pos_pizza_service"></a> [pos\_pizza\_service](#module\_pos\_pizza\_service) | ../../modules/pos_pizza_service | n/a |

## Resources

No resources.

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_build_date"></a> [build\_date](#input\_build\_date) | Date and time when the service is beeing build. | `string` | `"1970-01-01T00:00:00Z"` | no |
| <a name="input_build_git_sha"></a> [build\_git\_sha](#input\_build\_git\_sha) | GIT SHA of the commit that is build. | `string` | `"A100000000000000000000000000000000000000"` | no |
| <a name="input_env"></a> [env](#input\_env) | env = {<br/>      short = "Short name of the environment. Eg. `d`, `t`, `s`, `p`"<br/>      long  = "Long name of the environment. Eg. `dev`, `test`, `stage`, `prod`"<br/>    } | <pre>object({<br/>    short = string<br/>    long  = string<br/>  })</pre> | <pre>{<br/>  "long": "dev",<br/>  "short": "d"<br/>}</pre> | no |
| <a name="input_project"></a> [project](#input\_project) | project = {<br/>      short = "Short name of the project."<br/>      long  = "Long name of the project."<br/>    } | <pre>object({<br/>    short = string<br/>    long  = string<br/>  })</pre> | <pre>{<br/>  "long": "Pizza-Ordering-Service",<br/>  "short": "pos"<br/>}</pre> | no |
| <a name="input_service_version"></a> [service\_version](#input\_service\_version) | Version of the service. | `string` | `"0.1.0-alpha-119"` | no |

## Outputs

No outputs.
