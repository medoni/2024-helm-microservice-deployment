# Frontend App Module

This Terraform module deploys a Single-Page Application (SPA) using AWS S3 and CloudFront.

## Features

- **S3 Bucket**: Hosts static frontend files
- **CloudFront**: Global content delivery with HTTPS
- **Automated Build**: Builds the frontend and uploads to S3
- **Environment Variables**: Replaces environment variables using `envsubst` before upload
- **Optional Custom Domain**: Support for custom domains via Route53
- **Cache Invalidation**: Automatic CloudFront cache invalidation after upload

## Prerequisites

- AWS CLI must be installed and configured
- Node.js and npm/pnpm must be installed
- `envsubst` must be available (part of `gettext`)
- `jq` must be installed

### Installing Prerequisites

```bash
# Ubuntu/Debian
sudo apt-get update
sudo apt-get install -y gettext-base jq awscli

# macOS
brew install gettext jq awscli
```

## Usage

### Basic Configuration (without Custom Domain)

```hcl
module "frontend_app" {
  source = "../../modules/frontend_app"

  project = var.project
  env     = var.env

  frontend = {
    source_path = "${path.cwd}/../../../../src/frontend"
    build_path  = "build"
  }

  config = {
    pizza_api_url = module.pos_pizza_service.api_gateway_invoke_url
  }
}
```

### With Custom Domain

```hcl
module "frontend_app" {
  source = "../../modules/frontend_app"

  project = var.project
  env     = var.env

  frontend = {
    source_path = "${path.cwd}/../../../../src/frontend"
    build_path  = "build"
  }

  config = {
    pizza_api_url = module.pos_pizza_service.api_gateway_invoke_url
  }

  domain_name          = "app.example.com"
  route53_zone_id      = "Z1234567890ABC"
  acm_certificate_arn  = "arn:aws:acm:us-east-1:123456789012:certificate/..."
}
```

**Important**: The ACM certificate must be created in the `us-east-1` region, as CloudFront only accepts certificates from this region.

## How It Works

1. **Build Process**: The module executes a build script that:
   - Builds the frontend using npm/pnpm
   - Processes `config.example.js` with `envsubst` and converts it to `config.js`
   - Replaces the `CONFIG_PIZZA_API_URL` environment variable

2. **Upload to S3**:
   - Static assets are uploaded with cache-control for 1 year
   - HTML and config.js are uploaded with no-cache (for quick updates)

3. **CloudFront**:
   - Distributes content globally
   - Redirects HTTP to HTTPS
   - Supports SPA routing (404 → index.html)
   - Cache is invalidated after each upload

## Configuring Environment Variables

The `static/config.example.js` file in the frontend should have the following format:

```javascript
(function (window) {
  window.__env = window.__env || {};

  window.__env.pizzaApiUrl = '$CONFIG_PIZZA_API_URL';
})(this);
```

The `$CONFIG_PIZZA_API_URL` variable will be replaced with the actual value during build.

## Outputs

- `cloudfront_distribution_id`: CloudFront distribution ID
- `cloudfront_domain_name`: CloudFront distribution domain name
- `s3_bucket_name`: S3 bucket name
- `frontend_url`: Complete URL to the frontend (either custom domain or CloudFront domain)

## Cache Strategy

- **Static Assets** (JS, CSS, images): Cached for 1 year
- **HTML and config.js**: No cache (no-cache, no-store, must-revalidate)

This enables fast application updates while static assets are optimally cached.

## Security

- S3 bucket is not publicly accessible
- Access only via CloudFront with Origin Access Control (OAC)
- Encryption at rest (AES256)
- Versioning enabled
- HTTPS-only access (HTTP redirects to HTTPS)

## Troubleshooting

### Build Fails

Check if all prerequisites are installed:
```bash
which npm pnpm envsubst jq aws
```

### CloudFront Shows Old Version

Cache invalidation takes a few minutes. Check the status:
```bash
aws cloudfront list-invalidations --distribution-id YOUR_DIST_ID
```

### Custom Domain Doesn't Work

1. Verify the ACM certificate was created in `us-east-1`
2. Check if the Route53 zone is correct
3. Wait for DNS propagation (can take up to 48h)
