FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app
EXPOSE 5000

COPY . ./

RUN dotnet restore ./src/Fetcharr.sln
RUN dotnet publish -c Release -o out ./src/Fetcharr.sln

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Fetcharr.API.dll"]