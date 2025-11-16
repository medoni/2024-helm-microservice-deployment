resource "aws_route53_record" "frontend_app" {
  count = local.use_custom_domain ? 1 : 0

  zone_id = var.route53_zone_id
  name    = var.domain_name
  type    = "A"

  alias {
    name                   = aws_cloudfront_distribution.frontend_app.domain_name
    zone_id                = aws_cloudfront_distribution.frontend_app.hosted_zone_id
    evaluate_target_health = false
  }
}

resource "aws_route53_record" "frontend_app_ipv6" {
  count = local.use_custom_domain ? 1 : 0

  zone_id = var.route53_zone_id
  name    = var.domain_name
  type    = "AAAA"

  alias {
    name                   = aws_cloudfront_distribution.frontend_app.domain_name
    zone_id                = aws_cloudfront_distribution.frontend_app.hosted_zone_id
    evaluate_target_health = false
  }
}
