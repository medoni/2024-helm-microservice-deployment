resource "aws_s3_bucket" "frontend_app" {
  bucket = local.bucket_name
}

resource "aws_s3_bucket_public_access_block" "frontend_app" {
  bucket = aws_s3_bucket.frontend_app.id

  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_s3_bucket_versioning" "frontend_app" {
  bucket = aws_s3_bucket.frontend_app.id

  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "frontend_app" {
  bucket = aws_s3_bucket.frontend_app.id

  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_policy" "frontend_app" {
  bucket = aws_s3_bucket.frontend_app.id
  policy = data.aws_iam_policy_document.s3_cloudfront_policy.json
}

data "aws_iam_policy_document" "s3_cloudfront_policy" {
  statement {
    sid = "AllowCloudFrontServicePrincipal"

    principals {
      type        = "Service"
      identifiers = ["cloudfront.amazonaws.com"]
    }

    actions = [
      "s3:GetObject"
    ]

    resources = [
      "${aws_s3_bucket.frontend_app.arn}/*"
    ]

    condition {
      test     = "StringEquals"
      variable = "AWS:SourceArn"
      values   = [aws_cloudfront_distribution.frontend_app.arn]
    }
  }
}
