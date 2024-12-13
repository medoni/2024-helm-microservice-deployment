resource "aws_dynamodb_table" "pos_pizza_service_menus_table" {
  name           = "${var.project.short}-${var.env.short}-pizza-service-menus"
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "id"

  attribute {
    name = "id"
    type = "S"
  }

  attribute {
    name = "active"
    type = "N"
  }

  global_secondary_index {
    name               = "active"
    hash_key           = "active"
    projection_type    = "INCLUDE"
    non_key_attributes = ["active", "payload"]
  }
}

resource "aws_iam_policy" "pos_pizza_service_dynamodb_menus_table_access_policy" {
  name = "${var.project.short}-${var.env.short}-pizza-service-dynamodb-menus-access-policy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect   = "Allow"
        Action   = [
          "dynamodb:DescribeTable",
          "dynamodb:BatchWriteItem",
          "dynamodb:BatchGetItem",
          "dynamodb:PutItem",
          "dynamodb:GetItem",
          "dynamodb:UpdateItem",
          "dynamodb:DeleteItem",
          "dynamodb:Scan",
          "dynamodb:Query"
        ]
        Resource = [
          aws_dynamodb_table.pos_pizza_service_menus_table.arn,
          "${aws_dynamodb_table.pos_pizza_service_menus_table.arn}/index/*"
        ]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "pos_pizza_service_menus_table_policy_attachment" {
  role       = module.pos_pizza_service.iam_role_exec_role
  policy_arn = aws_iam_policy.pos_pizza_service_dynamodb_menus_table_access_policy.arn
}
