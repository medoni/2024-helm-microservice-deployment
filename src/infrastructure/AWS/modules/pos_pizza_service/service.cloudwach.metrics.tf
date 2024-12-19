variable "metric_well_known_sns_message_types" {
  type = list(string)
  description = "Wellknown message types to create cloud watch metrics from"
  default = [
    "POS.Domains.Customer.Abstractions.Carts.Events.CartCreatedEvent",
    "POS.Domains.Customer.Abstractions.Carts.Events.CartCheckedOutEvent",
    "POS.Domains.Customer.Abstractions.Orders.Events.OrderCreatedByCheckoutEvent"
  ]
}

resource "aws_cloudwatch_log_metric_filter" "pizza_service_published_sns_events_by_type" {
  for_each = toset(var.metric_well_known_sns_message_types)

  name           = "${var.project.short}-${var.env.short}-pizza-service-published-messages-${each.value}"
  log_group_name = module.pos_pizza_service.lambda_log_group_name
  pattern        = "{ $.Message = \"Successful published message of type '${ each.value }'.\" }"

  metric_transformation {
    name      = "Event ${ each.value }"
    namespace = "${var.project.short}-${var.env.short}-pizza-service-published-messages"
    value     = "1"
  }
}
