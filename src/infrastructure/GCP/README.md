# Google Cloud Pizza Ordering System Infrastructure

This folder contains Terraform configurations for deploying the Pizza Ordering System to Google Cloud Platform (GCP).

## Structure

- `/modules` - Reusable Terraform modules 
  - `/gcp_cloud_run_based_on_dockerfile` - Module for deploying Docker containers to Cloud Run
  - `/pos_pizza_service` - Module for deploying the Pizza Ordering Service and related resources
- `/envs` - Environment-specific configurations
  - `/dev` - Development environment configuration

## Prerequisites

1. [Google Cloud CLI](https://cloud.google.com/sdk/docs/install) installed and configured
2. [Terraform](https://www.terraform.io/downloads.html) installed
3. A Google Cloud Project with billing enabled
4. Required APIs enabled in your GCP project:
   - Cloud Run API
   - Artifact Registry API
   - Firestore API
   - Pub/Sub API
   - Monitoring API
   - Cloud Endpoints API
   - API Gateway API

## Creating Storage Bucket for Terraform State

Before you can use the Terraform configurations, you need to create a GCS bucket for storing the Terraform state:

```bash
# Set your project ID
PROJECT_ID=pizza-ordering-system-dev

# Create a bucket for Terraform state
gcloud storage buckets create gs://${PROJECT_ID}-tfstate --project=$PROJECT_ID --location=us-central1
```

## Deployment Steps

1. Navigate to the environment directory you want to deploy:

```bash
cd envs/dev
```

2. Initialize Terraform:

```bash
terraform init
```

3. Apply the configuration:

```bash
terraform plan
terraform apply
```

## Environment Variables

You may need to set the following environment variables for Terraform:

```bash
export GOOGLE_PROJECT=pizza-ordering-system-dev
export GOOGLE_REGION=us-central1
```

## Components

The infrastructure deploys the following components:

- **Cloud Run Service**: Runs the containerized Pizza Service application
- **Firestore Database**: NoSQL database for storing menus, carts, and orders
- **Pub/Sub Topics and Subscriptions**: For event-driven messaging
- **Cloud Monitoring Dashboard**: For monitoring service metrics
- **API Gateway**: For providing a unified API interface
- **Cloud Endpoints**: For API documentation and management

## Equivalent AWS Components

| GCP Component | AWS Equivalent |
|---------------|----------------|
| Cloud Run | Lambda/ECS |
| Firestore | DynamoDB |
| Pub/Sub | SNS |
| Cloud Monitoring | CloudWatch |
| Cloud Endpoints/API Gateway | API Gateway |
| Artifact Registry | ECR |