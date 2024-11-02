#syntax=docker/dockerfile:1.7-labs
# build context /src

ARG BUILD_VERSION="0.1.0-alpha-1"
ARG BUILD_DATE="1970-01-01T00:00:00Z"
ARG GIT_SHA="0000000000000000000000000000000000000000"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    COPY --parents **/*.Build.props .
    COPY --parents **/*.Build.targets .
    COPY --parents **/*.csproj .
    RUN dotnet restore backend/Infrastructure/POS.Persistence.PostgreSql.DbMigrations/*.csproj

    COPY / .
    RUN dotnet build backend/Infrastructure/POS.Persistence.PostgreSql.DbMigrations/*.csproj

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
    ARG BUILD_VERSION
    ARG BUILD_DATE
    ARG GIT_SHA

    WORKDIR /app
    COPY --from=build /src/backend/*.Build.props .
    COPY --from=build /src/backend/*.Build.targets .
    COPY --from=build /src/backend/Infrastructure/POS.Persistence.PostgreSql.DbMigrations db-migrations
    RUN dotnet tool install -g dotnet-ef --version 8.0.10

    WORKDIR /app/db-migrations

    # verify build
    RUN CONNECTION_STRING="Host=localhost" /root/.dotnet/tools/dotnet-ef dbcontext info --no-build -v

    LABEL org.label-schema.schema-version="1.0"
    LABEL org.label-schema.version="$BUILD_VERSION"
    LABEL org.label-schema.build-date="$BUILD_DATE"
    LABEL org.label-schema.name="pos-db-migrations"
    LABEL org.label-schema.description="Responsible for database migrations for the PizzaService"
    LABEL org.label-schema.url="https://github.com/medoni/2024-helm-microservice-deployment/tree/master/src/backend/Infrastructure/POS.PostgreSql.DbMigrations/"
    LABEL org.label-schema.vcs-ref="$GIT_SHA"
    LABEL org.label-schema.docker.cmd='docker build --build-arg BUILD_VERSION=0.1.2-alpha.2 --build-arg GIT_SHA=7e7987836646d85d9dcd5d470cf60436244a6f7d --build-arg "BUILD_DATE=$( date -u +"%Y-%m-%dT%H:%M:%SZ" )" .'

    ENTRYPOINT /root/.dotnet/tools/dotnet-ef database update --no-build -v
