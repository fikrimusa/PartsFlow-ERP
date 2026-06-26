FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY backend/PartsFlow.Api/PartsFlow.Api.csproj backend/PartsFlow.Api/
RUN dotnet restore backend/PartsFlow.Api/PartsFlow.Api.csproj

COPY backend/PartsFlow.Api/ backend/PartsFlow.Api/
RUN dotnet publish backend/PartsFlow.Api/PartsFlow.Api.csproj \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

CMD ["sh", "-c", "dotnet PartsFlow.Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
