# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first for layer-cached restore
COPY Unscrambler/Unscrambler.csproj          Unscrambler/
COPY Unscrambler.Web/Unscrambler.Web.csproj  Unscrambler.Web/
RUN dotnet restore Unscrambler.Web/Unscrambler.Web.csproj

# Copy shared business logic and web project source
COPY Unscrambler/AnagramSolver.cs  Unscrambler/
COPY Unscrambler/Stations.cs       Unscrambler/
COPY Unscrambler.Web/              Unscrambler.Web/

RUN dotnet publish Unscrambler.Web/Unscrambler.Web.csproj \
    --configuration Release \
    --runtime linux-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    --output /app/publish

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
# runtime-deps is sufficient because self-contained publish bundles the .NET runtime
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0 AS runtime
WORKDIR /app

# Create a non-root user
RUN groupadd --system --gid 1001 appgroup \
 && useradd  --system --uid 1001 --gid 1001 --no-create-home appuser

COPY --from=build --chown=appuser:appuser /app/publish .

USER appuser

# Kestrel listens on 8080 in container contexts (DOTNET_RUNNING_IN_CONTAINER=true)
# Kestrel default ports in .NET 8+ containers are 8080 for HTTP and 8081 for HTTPS, so we expose both
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["./Unscrambler.Web"]
