# Reservly — Reservation System (MVP)

Microservices-based reservation system built with **.NET 8, MongoDB, RabbitMQ, Redis, Docker, and Kubernetes**.  
It allows organizers to publish events and attendees to reserve seats with an expiration window, pay online (Stripe) or in cash, and confirm with a **QR code** at check-in.

## 🚀 Features (MVP)
- Event publishing with capacity & sessions
- Reservation with hold window (anti-overbooking)
- Payment via Stripe (online) or cash
- Confirmation with email + QR code
- Check-in validation via QR token
- Microservices architecture + CQRS

## 🛠️ Tech Stack
- .NET 8 (minimal APIs, MediatR, CQRS)
- MongoDB (aggregations, indexes)
- RabbitMQ (async messaging)
- Redis (reservation TTL)
- Docker Compose (local dev)
- Kubernetes (future deployment)
- Observability: Serilog + OpenTelemetry + Prometheus

## 📂 Structure
- src/
    - BuildingBlocks/ # Shared libs (contracts, utils)
    - DbMigrator/ # Console app for MongoDB migrations
    - EventService/ # Minimal API for managing events
- deploy/
    - docker-compose.yml

## 🔧 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Docker Desktop](https://www.docker.com/)

### Run infrastructure
```bash

cd deploy

docker compose up -d    

MongoDB: mongodb://root:rootpassword@localhost:27017

Mongo Express: http://localhost:8081

RabbitMQ: http://localhost:15672 (guest/guest)