#!/bin/bash
set -e

echo "=== Frontend Build and Upload Script ==="
echo "Source Path: $FRONTEND_SOURCE_PATH"
echo "Build Path: $FRONTEND_BUILD_PATH"
echo "S3 Bucket: $S3_BUCKET_NAME"
echo "CloudFront Distribution: $CLOUDFRONT_DIST_ID"

# Check if pnpm is available
if ! command -v pnpm &> /dev/null; then
    echo "Error: pnpm not found. Please install pnpm first."
    exit 1
fi

# Navigate to frontend source directory
cd "$FRONTEND_SOURCE_PATH"

# Install dependencies if needed
if [ ! -d "node_modules" ]; then
    echo "Installing dependencies with pnpm..."
    pnpm install
fi

# Build the frontend
echo "Building frontend with pnpm..."
pnpm run build

# Process config.example.js with envsubst
if [ -f "${FRONTEND_BUILD_PATH}/config.example.js" ]; then
    echo "Processing config.example.js with envsubst..."
    export CONFIG_PIZZA_API_URL
    envsubst < "${FRONTEND_BUILD_PATH}/config.example.js" > "${FRONTEND_BUILD_PATH}/config.js"
    echo "Created config.js with API URL: $CONFIG_PIZZA_API_URL"
fi

# Upload to S3
echo "Uploading to S3 bucket: $S3_BUCKET_NAME..."
aws s3 sync "$FRONTEND_BUILD_PATH" "s3://${S3_BUCKET_NAME}/" \
    --delete \
    --cache-control "public, max-age=31536000, immutable" \
    --exclude "*.html" \
    --exclude "config.js"

# Upload HTML files with no-cache (for SPA updates)
echo "Uploading HTML and config files with no-cache..."
aws s3 sync "$FRONTEND_BUILD_PATH" "s3://${S3_BUCKET_NAME}/" \
    --cache-control "no-cache, no-store, must-revalidate" \
    --exclude "*" \
    --include "*.html" \
    --include "config.js"

# Invalidate CloudFront cache
echo "Invalidating CloudFront cache..."
aws cloudfront create-invalidation \
    --distribution-id "$CLOUDFRONT_DIST_ID" \
    --paths "/*" \
    | jq -r '.Invalidation.Id' \
    | xargs -I {} echo "CloudFront invalidation created: {}"

echo "=== Build and Upload Complete ==="
