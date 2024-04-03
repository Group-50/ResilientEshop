# Sample Microservice Overview

## Reasoning

### Why build our own microservice application?

- Gain experience building microservice applications
- Have full control over the design and implementation
- Adding or removing features is straightforward
- Don't have to deal with existing design decisions

#### Drawbacks

- Have to design and build it ourselves
- We are making the design and implementation decisions

### Why build an E-Commerce app?

- Plenty of available open source examples
- Clearly defined domain boundaries
- Can be as simple or complex as we want
- Flexible
  - Architecture
  - Communication Protocols
    - Bus
    - Queue
    - GRPC
    - HTTP / HTTPS
  - Service Design

## Design

### Projects

- Catalog Service
- Basket Service
- Order Service
- Authentication Service
- Service Bus
- Gateway
- Building Blocks

## Objectives

- Create a sample microservice application for demonstrating the test bed
- Learn how to build microservice applications
- Learn how to configure and deploy applications
- Test various architecture patterns and how to work with them
- Provide an environment for testing and developing the monitoring service
- Provide an environment for testing chaos engineering
- Provide an environment for testing and developing the orchestrator service
- Provide a scaffold for the template
- Learn how to develop unit tests
- Learn how to develop integration tests
- Learn how to develop end-to-end tests
- Learn how to build and configure CI / CD pipelines

## Testing Objectives

- Application level resilience
- Working with application gateways
- Working with authentication
- Communication protocols
  - HTTP/HTTPS
  - GRPC
- Service Bus
  - RabbitMQ
  - MassTransit
- Working with events
- Working with containers