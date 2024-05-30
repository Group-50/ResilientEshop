FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY resilient-eshop.sln .
COPY /src/CatalogService/CatalogService.csproj src/CatalogService/CatalogService.csproj
COPY /src/ServiceDefaults/ServiceDefaults.csproj src/ServiceDefaults/ServiceDefaults.csproj

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release


COPY src/CatalogService/ src/CatalogService/
COPY src/ServiceDefaults/ src/ServiceDefaults/

RUN dotnet restore "src/CatalogService/CatalogService.csproj"

RUN dotnet build "src/CatalogService/CatalogService.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/CatalogService/CatalogService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.dll"]
