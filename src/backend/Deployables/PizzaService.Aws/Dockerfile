#syntax=docker/dockerfile:1.7-labs
# build context /src

ARG BUILD_VERSION="0.1.0-alpha-1"
ARG BUILD_DATE="1970-01-01T00:00:00Z"
ARG GIT_SHA="0000000000000000000000000000000000000000"

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
    ARG BUILD_VERSION
    ARG GIT_SHA

    WORKDIR /src
    COPY --parents **/*.Build.props .
    COPY --parents **/*.Build.targets .
    COPY --parents **/*.csproj .
    RUN dotnet restore backend/Deployables/PizzaService.Aws/*.csproj

    COPY . .
    RUN dotnet publish backend/Deployables/PizzaService.Aws/*.csproj \
        -c Release \
        --self-contained false \
        -r linux-x64 \
        -o /app/publish \
        /p:MINVERVERSIONOVERRIDE=$BUILD_VERSION \
        /p:SourceRevisionId=$GIT_SHA

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS final
    ARG BUILD_VERSION
    ARG BUILD_DATE
    ARG GIT_SHA

    WORKDIR /app
    COPY --from=build /app/publish .

    LABEL org.label-schema.schema-version="1.0"
    LABEL org.label-schema.version="$BUILD_VERSION"
    LABEL org.label-schema.build-date="$BUILD_DATE"
    LABEL org.label-schema.name="2024-helm-microservice-deployment/pizza-service-aws"
    LABEL org.label-schema.description="Responsible for providing basic access to the Pizza-Ordering-System"
    LABEL org.label-schema.url="https://github.com/medoni/2024-helm-microservice-deployment/tree/master/src/backend/Deployables/PizzaService.Aws"
    LABEL org.label-schema.vcs-ref="$GIT_SHA"
    LABEL org.label-schema.docker.cmd='docker build --build-arg BUILD_VERSION=0.1.2-alpha.2 --build-arg GIT_SHA=7e7987836646d85d9dcd5d470cf60436244a6f7d --build-arg "BUILD_DATE=$( date -u +"%Y-%m-%dT%H:%M:%SZ" )" .'

    ENTRYPOINT dotnet PizzaService.Aws.dll
