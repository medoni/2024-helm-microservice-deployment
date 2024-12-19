resource "aws_sns_topic" "pizza_service_topic" {
  name = "${var.project.short}-${var.env.short}-pizza-service-topic"

  tracing_config = "Active"
}

resource "aws_iam_policy" "pizza_service_sns_topic_policy" {
  name = "${var.project.short}-${var.env.short}-pizza-service-topic-policy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect   = "Allow"
        Action   = [
          "sns:Publish"
        ]
        Resource = [
          aws_sns_topic.pizza_service_topic.arn
        ]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "pizza_service_sns_topic_policy_attachment" {
  role       = module.pos_pizza_service.iam_role_exec_role
  policy_arn = aws_iam_policy.pizza_service_sns_topic_policy.arn
}
