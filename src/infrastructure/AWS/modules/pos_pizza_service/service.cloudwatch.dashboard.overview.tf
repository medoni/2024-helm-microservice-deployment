resource "aws_cloudwatch_dashboard" "overview" {
  dashboard_name = "Pizza-Service-Overview"

  dashboard_body = jsonencode({
    widgets = [
      {
        "height": 2,
        "width": 24,
        "y": 0,
        "x": 0,
        "type": "text",
        "properties": {
            "markdown": <<-EOF
# Pizza-Ordering-Service
[Api-Gateway-Stage](https://${data.aws_region.current.name}.console.aws.amazon.com/apigateway/main/apis/${aws_api_gateway_rest_api.pos_pizza_service_rest_api.id}/stages?api=${aws_api_gateway_rest_api.pos_pizza_service_rest_api.id}&region=${data.aws_region.current.name}) | [Lambda](https://${data.aws_region.current.name}.console.aws.amazon.com/lambda/home?region=${data.aws_region.current.name}#/functions/${module.pos_pizza_service.lambda_function_name}?tab=image) | [DynamoDb-Tables](https://${data.aws_region.current.name}.console.aws.amazon.com/dynamodbv2/home?region=${data.aws_region.current.name}#item-explorer) | [Documentation](https://github.com/medoni/2024-helm-microservice-deployment/tree/master/src/backend/Deployables/PizzaService.Aws) | [Edit Source](https://github.com/medoni/2024-helm-microservice-deployment/blob/master/src/infrastructure/AWS/modules/pos_pizza_service/service.cloudwatch.dashboard.overview.tf)
EOF
            "background": "solid"
        }
      },
      {
        "height": 4,
        "width": 9,
        "y": 2,
        "x": 0,
        "type": "metric",
        "properties": {
            "metrics": [
                [ "AWS/ApiGateway", "Count", "ApiName", aws_api_gateway_rest_api.pos_pizza_service_rest_api.name, "Stage", "api", { "label": "Total requests", "region": data.aws_region.current.name } ],
                [ ".", "5XXError", ".", ".", ".", ".", { "region": data.aws_region.current.name, "yAxis": "left" } ],
                [ ".", "4XXError", ".", ".", ".", ".", { "region": data.aws_region.current.name, "color": "#dfb52c" } ]
            ],
            "sparkline": false,
            "view": "singleValue",
            "region": data.aws_region.current.name,
            "title": "API requests",
            "period": 300,
            "stat": "Sum",
            "liveData": true,
            "stacked": false,
            "yAxis": {
                "left": {
                    "min": 0,
                    "max": 10
                }
            },
            "annotations": {
                "horizontal": [
                    {
                        "color": "#b2df8d",
                        "label": "Untitled annotation",
                        "value": 1,
                        "fill": "above"
                    }
                ]
            },
            "singleValueFullPrecision": false,
            "setPeriodToTimeRange": true,
            "trend": false
        }
      },
      {
        "height": 4,
        "width": 14,
        "y": 2,
        "x": 9,
        "type": "metric",
        "properties": {
            "metrics": [
                [ aws_cloudwatch_log_metric_filter.domain_events.metric_transformation[0].namespace, "DomainEventsCount", "DomainEventName", "POS.Domains.Customer.Abstractions.Carts.Events.CartCreatedEvent", { "region": data.aws_region.current.name, "label": "Carts created" } ],
                [ "...", "POS.Domains.Customer.Abstractions.Orders.Events.OrderCreatedByCheckoutEvent", { "label": "Orders created (by cart checkout)", "color": "#08aad2" } ],
                [ "...", "POS.Domains.Customer.Abstractions.Carts.Events.CartCheckedOutEvent", { "stat": "Average", "label": "Orders payed (Not implemented)" } ],
                [ "...", { "stat": "Average", "label": "Orders delivered (Not implemented)", "color": "#69ae34" } ]
            ],
            "sparkline": true,
            "view": "singleValue",
            "region": data.aws_region.current.name,
            "stat": "Sum",
            "period": 300,
            "liveData": true,
            "title": "Domain Events"
        }
      },
      {
        "height": 6,
        "width": 24,
        "y": 6,
        "x": 0,
        "type": "log",
        "properties": {
            "query": "SOURCE '/aws/lambda/${module.pos_pizza_service.lambda_function_name}' | fields @timestamp, Message, Category, @xrayTraceId\n| filter LogLevel like /(Error|Critical)/\n| sort @timestamp desc\n| limit 10000",
            "region": data.aws_region.current.name,
            "stacked": false,
            "title": "Error messages",
            "view": "table"
        }
      },
      {
        "height": 3,
        "width": 24,
        "y": 12,
        "x": 0,
        "type": "metric",
        "properties": {
            "metrics": [
                [ "AWS/Lambda", "Invocations", "FunctionName", "${module.pos_pizza_service.lambda_function_name}", { "yAxis": "left", "label": "Invocations" } ],
                [ ".", "ConcurrentExecutions", ".", ".", { "label": "ConcurrentExecutions (P95)", "stat": "p95" } ],
                [ "...", { "stat": "Maximum", "label": "ConcurrentExecutions(Max)" } ],
                [ ".", "Throttles", ".", ".", { "label": "Throttles" } ],
                [ ".", "Errors", ".", ".", { "label": "Errors" } ]
            ],
            "sparkline": false,
            "view": "singleValue",
            "region": data.aws_region.current.name,
            "stat": "Sum",
            "period": 300,
            "liveData": true,
            "setPeriodToTimeRange": true,
            "trend": false,
            "title": "Lambda"
        }
      },
      {
        "height": 3,
        "width": 24,
        "y": 15,
        "x": 0,
        "type": "metric",
        "properties": {
            "metrics": [
                [ "AWS/ApiGateway", "Latency", "ApiName", "${aws_api_gateway_rest_api.pos_pizza_service_rest_api.name}", { "stat": "p95", "label": "Latency (P95)" } ],
                [ "...", { "label": "Latency (Max)" } ],
                [ ".", "Count", ".", ".", { "stat": "Sum", "label": "Request Count" } ],
                [ ".", "5XXError", ".", "." ],
                [ ".", "4XXError", ".", "." ]
            ],
            "sparkline": false,
            "view": "singleValue",
            "region": data.aws_region.current.name,
            "setPeriodToTimeRange": true,
            "trend": false,
            "stat": "Maximum",
            "period": 300,
            "title": "Api Gateway"
        }
      },
    ]
  })
}
