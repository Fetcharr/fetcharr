# syntax=docker/dockerfile:1.9-labs

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app
EXPOSE 5000

COPY --parents ./src/*.props ./src/**/*.csproj ./
RUN dotnet restore src/API/src/Fetcharr.API.csproj

COPY . .
RUN dotnet publish src/API/src/Fetcharr.API.csproj --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

ENV FETCHARR_BASE_DIR="/app/fetcharr" \
    FETCHARR_CONFIG_DIR="/config"

RUN set -ex; \
    mkdir -p /config && chown 1000:1000 /config;

COPY --from=build-env --chown=1000:1000 /app /app/fetcharr

USER 1000:1000
VOLUME /config

ENTRYPOINT ["dotnet", "/app/fetcharr/Fetcharr.API.dll"]