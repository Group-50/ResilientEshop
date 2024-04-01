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

- getProducts
- getProductsById
- getProductsByBrandId
- getProductsByCatalogTypeId

- getCatalogBrands
- getCatalogBrandById


- getCatalogTypes


## Events Emitted

## Events Consumed

## API Endpoints

- api/products
  - Get
- api/products{productId}
  - Post
  - Get
- api/brands
  - Get
- api/brands{brandId}
  - Get
- api/catalogs
  - Get

## Models

### CatalogItem

### CatalogItemPrice

### CatalogType

### CatalogBrand

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

