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
[Api-Gateway-Stage](https://eu-central-1.console.aws.amazon.com/apigateway/main/apis/m9s12tlge2/stages?api=m9s12tlge2&region=eu-central-1) | [Lambda](https://eu-central-1.console.aws.amazon.com/lambda/home?region=eu-central-1#/functions/pos-d-pizza-service?tab=image) | [DynamoDb-Tables](https://eu-central-1.console.aws.amazon.com/dynamodbv2/home?region=eu-central-1#item-explorer) | [Documentation](https://github.com/medoni/2024-helm-microservice-deployment/tree/master/src/backend/Deployables/PizzaService.Aws) | [Edit Source]()
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
                [ "AWS/ApiGateway", "Count", "ApiName", "pos-d-pizza-service", "Stage", "api", { "label": "Total requests", "region": "eu-central-1" } ],
                [ ".", "5XXError", ".", ".", ".", ".", { "region": "eu-central-1", "yAxis": "left" } ],
                [ ".", "4XXError", ".", ".", ".", ".", { "region": "eu-central-1", "color": "#dfb52c" } ]
            ],
            "sparkline": false,
            "view": "singleValue",
            "region": "eu-central-1",
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
                [ "pos-d-pizza-service", "DomainEventsCount", "DomainEventName", "POS.Domains.Customer.Abstractions.Carts.Events.CartCreatedEvent", { "region": "eu-central-1", "label": "Carts created" } ],
                [ "...", "POS.Domains.Customer.Abstractions.Orders.Events.OrderCreatedByCheckoutEvent", { "label": "Orders created (by cart checkout)", "color": "#08aad2" } ],
                [ "...", "POS.Domains.Customer.Abstractions.Carts.Events.CartCheckedOutEvent", { "stat": "Average", "label": "Orders payed (Not implemented)" } ],
                [ "...", { "stat": "Average", "label": "Orders delivered (Not implemented)", "color": "#69ae34" } ]
            ],
            "sparkline": true,
            "view": "singleValue",
            "region": "eu-central-1",
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
            "query": "SOURCE '/aws/lambda/pos-d-pizza-service' | fields @timestamp, Message, Category, @xrayTraceId\n| filter LogLevel like /(Error|Critical)/\n| sort @timestamp desc\n| limit 10000",
            "region": "eu-central-1",
            "stacked": false,
            "title": "Pizza-Service Error messages",
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
                [ "AWS/Lambda", "Invocations", "FunctionName", "pos-d-pizza-service", { "yAxis": "left", "label": "Invocations" } ],
                [ ".", "ConcurrentExecutions", ".", ".", { "label": "ConcurrentExecutions (P95)", "stat": "p95" } ],
                [ "...", { "stat": "Maximum", "label": "ConcurrentExecutions(Max)" } ],
                [ ".", "Throttles", ".", ".", { "label": "Throttles" } ],
                [ ".", "Errors", ".", ".", { "label": "Errors" } ]
            ],
            "sparkline": false,
            "view": "singleValue",
            "region": "eu-central-1",
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
                [ "AWS/ApiGateway", "Latency", "ApiName", "pos-d-pizza-service", { "stat": "p95", "label": "Latency (P95)" } ],
                [ "...", { "label": "Latency (Max)" } ],
                [ ".", "Count", ".", ".", { "stat": "Sum", "label": "Request Count" } ],
                [ ".", "5XXError", ".", "." ],
                [ ".", "4XXError", ".", "." ]
            ],
            "sparkline": false,
            "view": "singleValue",
            "region": "eu-central-1",
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
