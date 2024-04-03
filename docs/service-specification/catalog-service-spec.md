# Catalog Service Specification

## Infrastructure

.Net 8 Web API

PostgreSQL Database

Service Bus - RabbitMQ

## NuGet Packages

Npgsql.EntityFrameworkCore.PostgreSQL

Npgsql.OpenTelemetry

MassTransit.RabbitMQ

OpenTelemetry

OpenTelemetry.Extensions.Hosting

OpenTelemetry.Instrumentation.AspNetCore

OpenTelemetry.Instrumentation.EventCounter

OpenTelemetry.Instrumentation.Runtime

OpenTelemetry.Exporter.Prometheus.AspNetCore

OpenTelemetry.Exporter.Zipkin

OpenTelemetry.Exporter.Jaeger

Serilog

Polly


## Queries Handled

- **GetProducts** - Get all products. Returns list of **ProductDTO**

- **GetProductById** - Get a product by Id. Returns **ProductDTO**

- **GetProductsById** - Get products by Ids. Returns list of **ProductDTO**

- **GetProductsByBrandId** - Get products by BrandId. Returns list of **ProductDTO**

- **GetProductsByTypeId** - Get products by Catalog Type Id. Returns list of **ProductDTO**

- **GetCatalogBrands** - Get all Brands. Returns **CatalogBrandsDTO**

- **GetCatalogTypes** - Get all Catalog Types. Returns **CatalogTypesDTO**


## Events Emitted

## Events Consumed

## API Endpoints

| HTTP | Endpoint                              | Action                   | Auth |
|------|---------------------------------------|--------------------------|------|
| GET  | api/products                          | Get All Products         |      |
| GET  | api/products/by                       | Get Products by Ids      |      |
| GET  | api/products/{productId}              | Get Product by Id        |      |
| GET  | api/products/type/{typeId}            | Get Products by Type Id  |      |
| GET  | api/products/type/all/brand/{brandId} | Get Products by Brand Id |      |
| GET  | api/catalogbrands                     | Get Brands               |      |
| GET  | api/catalogtypes                      | Get all Catalog Types    |      |

## Models

### CatalogItem

| Property Name  | Property Type                 | Default Value |
|----------------|-------------------------------|---------------|
| Id             | Int                           |               |
| Name           | String                        |               |
| Description    | String                        |               |
| ImageUrl       | String                        |               |
| CatalogTypeId  | Int                           |               |
| CatalogType    | CatalogType                   |               |
| CatalogBrandId | Int                           |               |
| CatalogBrand   | CatalogBrand                  |               |
| PriceHistory   | ICollection<CatalogItemPrice> |               |

### CatalogItemPrice

| Property Name | Property Type | Default Value   |
|---------------|---------------|-----------------|
| Id            | Int           |                 |
| ProductId     | Int           |                 |
| EffectiveFrom | DateTime      |                 |
| CreatedOn     | DateTime      | DateTime.UtcNow |

### CatalogType

| Property Name | Property Type | Default Value |
|---------------|---------------|---------------|
| Id            | Int           |               |
| Type          | String        |               |

### CatalogBrand

| Property Name | Property Type | Default Value |
|---------------|---------------|---------------|
| Id            | Int           |               |
| Brand         | String        |               |

## DTOs

### ProductDTO


| Property Name | Property Type                 | Default Value |
|---------------|-------------------------------|---------------|
| Id            | Int                           |               |
| Name          | String                        |               |
| Description   | String                        |               |
| ImageUrl      | String                        |               |
| Type          | String                        |               |
| Brand         | String                        |               |
| Price         | Decimal                       |               |


### CatalogBrandDTO

| Property Name | Property Type                 | Default Value |
|---------------|-------------------------------|---------------|
| Id            | Int                           |               |
| BrandName     | String                        |               |

### CatalogTypeDTO

| Property Name | Property Type                 | Default Value |
|---------------|-------------------------------|---------------|
| Id            | Int                           |               |
| TypeName      | String                        |               |

## Event Emitted Types

## Event Consumed Types

## Open Telemetry

### Meters

### Counters

### Baggage

## Health & Metrics

### Health Checks

### Metrics

### Health & Metrics Endpoints

api/metrics

### Tags

## Options Patterns

### Open Telemetry Options

### Polly Options

