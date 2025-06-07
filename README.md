## Install on Kubernetes
```powershell
src/infrastructure/ArgoCD/bootstrap.ps1
```

## Run Locally with Docker Compose

You can start the entire application stack (backend, frontend, database, migrations, seed, and admin tools) using Docker Compose:

```powershell
cd src
docker-compose up --build
```

- Backend API: http://localhost:5000
- Frontend app: http://localhost:5174
- Postgres DB: localhost:5432 (user: postgres, password: postgres)
- pgweb (DB admin): http://localhost:5001

---

## Available Features

- **Pizza Ordering API**: RESTful backend for pizzas, carts, and orders. Written in C#/.NET
- **Frontend Web App**: Svelte-based UI for browsing menu, creating carts, and placing orders
- **Database Migrations & Seeding**: Automated DB setup and initial data population
- **Admin Tools**: pgweb for database inspection
- **Cloud/Infra Ready**: Helm charts and ArgoCD scripts for Kubernetes deployment
- **Monitoring**: AWS/GCP modules for metrics, logs, and dashboards (see `src/infrastructure`)
