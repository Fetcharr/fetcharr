# syntax=docker/dockerfile:1.9-labs

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app
EXPOSE 5000

COPY --parents ./src/*.props ./src/**/*.csproj ./
RUN dotnet restore src/API/src/Fetcharr.API.csproj

COPY . .
RUN dotnet publish src/API/src/Fetcharr.API.csproj --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
COPY --from=build-env /app /app

ENTRYPOINT ["dotnet", "/app/Fetcharr.API.dll"]