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
RUN addgroup --system --gid 1001 appgroup \
 && adduser  --system --uid 1001 --ingroup appgroup --no-create-home appuser

COPY --from=build --chown=appuser:appgroup /app/publish .

USER appuser

# Kestrel listens on 8080 in container contexts (DOTNET_RUNNING_IN_CONTAINER=true)
EXPOSE 8080

ENTRYPOINT ["./Unscrambler.Web"]
