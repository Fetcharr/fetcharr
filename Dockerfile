# syntax=docker/dockerfile:1.9-labs

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app
EXPOSE 5000

ARG TARGETARCH

COPY --parents ./src/*.props ./src/*.targets ./src/**/*.csproj ./
RUN dotnet restore src/API/src/Fetcharr.API.csproj -a $TARGETARCH

COPY . .
RUN dotnet publish src/API/src/Fetcharr.API.csproj -a $TARGETARCH --no-restore -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

ENV FETCHARR_BASE_DIR="/app/fetcharr" \
    FETCHARR_CONFIG_DIR="/config"

RUN set -ex; \
    mkdir -p /config && chown $APP_UID /config;

COPY --from=build-env --chown=$APP_UID /out /app/fetcharr

USER $APP_UID
VOLUME /config

ENTRYPOINT ["dotnet", "/app/fetcharr/Fetcharr.API.dll"]