resource "aws_cloudwatch_log_metric_filter" "domain_events" {
  name           = "DomainEventsCount"
  log_group_name = module.pos_pizza_service.lambda_log_group_name
  pattern        = "{ $.Message = \"Successful published message of type '*'.\" }"

  metric_transformation {
    name       = "DomainEventsCount"
    namespace  = "${var.project.short}-${var.env.short}-pizza-service"
    value      = "1"
    unit       = "Count"
    dimensions = {
      "DomainEventName" = "$.State.messageType"
    }
  }
}
