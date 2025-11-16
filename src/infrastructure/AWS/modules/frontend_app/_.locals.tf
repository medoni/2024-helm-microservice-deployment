locals {
  bucket_name = "${var.project.short}-${var.env.short}-frontend-app"

  use_custom_domain = var.domain_name != "" && var.route53_zone_id != "" && var.acm_certificate_arn != ""

  cloudfront_aliases = local.use_custom_domain ? [var.domain_name] : []
}
