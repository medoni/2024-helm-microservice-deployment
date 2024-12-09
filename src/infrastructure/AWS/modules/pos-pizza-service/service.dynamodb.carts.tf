resource "aws_dynamodb_table" "pos_pizza_service_carts_table" {
  name           = "pos-dev-pizza-service-carts"
  billing_mode   = "PAY_PER_REQUEST"
  hash_key       = "id"

  attribute {
    name = "id"
    type = "S"
  }
}

resource "aws_iam_policy" "pos_pizza_service_dynamodb_carts_table_access_policy" {
  name = "pos-dev-pizza-service-dynamodb-carts-access-policy"

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
          aws_dynamodb_table.pos_pizza_service_carts_table.arn,
          "${aws_dynamodb_table.pos_pizza_service_carts_table.arn}/index/*"
        ]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "pos_pizza_service_carts_table_policy_attachment" {
  role       = aws_iam_role.pos-pizza-service-lambda-exec-role.name
  policy_arn = aws_iam_policy.pos_pizza_service_dynamodb_carts_table_access_policy.arn
}
