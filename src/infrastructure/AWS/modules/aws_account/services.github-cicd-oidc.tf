# based on https://medium.com/@thiagosalvatore/using-terraform-to-connect-github-actions-and-aws-with-oidc-0e3d27f00123

resource "aws_iam_openid_connect_provider" "github" {
  url = "https://token.actions.githubusercontent.com"

  client_id_list = [
    "sts.amazonaws.com",
  ]

  thumbprint_list = ["ffffffffffffffffffffffffffffffffffffffff"]
}

data "aws_iam_policy_document" "github_oidc" {
  # https://docs.aws.amazon.com/IAM/latest/UserGuide/id_roles_create_for-idp_oidc.html#idp_oidc_Create_GitHub
  statement {
    actions = ["sts:AssumeRoleWithWebIdentity"]

    principals {
      type        = "Federated"
      identifiers = [aws_iam_openid_connect_provider.github.arn]
    }

    condition {
      test     = "StringEquals"
      values   = ["sts.amazonaws.com"]
      variable = "token.actions.githubusercontent.com:aud"
    }

    condition {
      test     = "StringLike"
      values   = ["repo:medoni/2024-helm-microservice-deployment:*"]
      variable = "token.actions.githubusercontent.com:sub"
    }
  }
}

resource "aws_iam_role" "github_deploy_role" {
  name               = "github-oidc-deploy-role"
  assume_role_policy = data.aws_iam_policy_document.github_oidc.json
}

data "aws_iam_policy_document" "github_deploy_policy" {
  statement {
    effect = "Allow"
    actions = [
      "apigateway:*",
      "cloudwatch:*",
      "ecr:*",
      "dynamodb:*",
      "events:*",
      "lambda:*",
      "logs:*",
      "s3:*",
      "sns:*"
    ]
    resources = ["*"]
  }

  statement {
    effect = "Allow"
    actions = [
      "iam:CreateRole",
      "iam:DeleteRole",
      "iam:UpdateRole",
      "iam:GetRole",
      "iam:PassRole",
      "iam:TagRole",
      "iam:AttachRolePolicy",
      "iam:DetachRolePolicy",
      "iam:PutRolePolicy",
      "iam:DeleteRolePolicy"
    ]
    resources = ["*"]
  }
  
  statement {
    effect = "Deny"
    actions = [
      "s3:DeleteBucket",
      "dynamodb:DeleteTable",
      "logs:DeleteLogGroup"
    ]
    resources = ["*"]
  }
}

resource "aws_iam_role_policy" "deploy" {
  name        = "github-oidc-deploy-policy"
  role        = aws_iam_role.github_deploy_role.id
  policy      = data.aws_iam_policy_document.github_deploy_policy.json
}
