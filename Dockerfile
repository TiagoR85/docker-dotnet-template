# syntax=docker/dockerfile:1

# Estágio 1: build com SDK completo
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Packages.props .
COPY src/Template.Api/Template.Api.csproj src/Template.Api/
RUN dotnet restore src/Template.Api/Template.Api.csproj

COPY src/Template.Api/ src/Template.Api/
RUN dotnet publish src/Template.Api/Template.Api.csproj \
    -c Release -o /app/publish --no-restore

# Estágio 2: runtime mínimo, sem SDK
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# curl é necessário apenas para o HEALTHCHECK do Dockerfile (ADR-0001)
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
EXPOSE 8080

# Usuário não-root built-in das imagens .NET (uid 1654) — ADR-0002
USER $APP_UID

HEALTHCHECK --interval=30s --timeout=3s --start-period=15s --retries=3 \
    CMD curl -fsS http://localhost:8080/health/live || exit 1

ENTRYPOINT ["dotnet", "Template.Api.dll"]
