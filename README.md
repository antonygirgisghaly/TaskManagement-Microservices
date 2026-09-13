# Task Management Microservices System

A task management platform built with a microservices architecture using .NET 10, demonstrating service independence, JWT-based authentication, inter-service communication, and containerized deployment with Docker.

## Overview

This project simulates a real-world microservices ecosystem composed of three independently deployable services, each following Onion (Clean) Architecture with strict separation of concerns. Every service owns its own database, communicates with others exclusively over HTTP, and can be built, tested, and deployed in isolation.

| Service | Responsibility | Database |
|---|---|---|
| **Identity.Api** | User registration, authentication, JWT issuance | `IdentityServiceDb` |
| **Tasks.Api** | Task CRUD operations, ownership & status management | `TasksServiceDb` |
| **Notifications.Api** | Records and serves notifications triggered by task events | `NotificationsServiceDb` |

## Architecture

Each service is structured using **Onion Architecture**, with dependencies flowing strictly inward:

```
Api  →  Infrastructure  →  Application  →  Domain
```

- **Domain** — Entities and enums only. No external dependencies.
- **Application** — DTOs, interfaces (contracts), business logic (services), and the Result pattern for error handling.
- **Infrastructure** — EF Core DbContext, repository implementations, external service clients (HttpClient), and security (JWT, password hashing).
- **Api** — Controllers, dependency injection wiring, and middleware configuration. The only layer aware of every other layer (Composition Root).

### Database per Service

Each service owns an isolated SQL Server database. There are **no foreign keys across service boundaries** — cross-service references (e.g., `AssignedToUserId` in Tasks, `UserId` in Notifications) are stored as plain GUIDs, trusted implicitly via a valid JWT rather than enforced at the database level. This preserves each service's ability to deploy, scale, and evolve independently.

### Why no Generic Repository / Unit of Work

This project deliberately uses **specific repositories** (`IUserRepository`, `ITaskRepository`, `INotificationRepository`) instead of a Generic Repository pattern. With one or two entities per service, a generic abstraction adds indirection without meaningful reuse, and typically still requires custom methods that break the "generic" contract anyway.

Similarly, **Unit of Work** is omitted — each service has a single repository whose operations are already wrapped by `DbContext.SaveChangesAsync()`, which functions as a Unit of Work on its own. Unit of Work becomes valuable when multiple repositories must commit atomically together, which does not occur in this system's current scope.

## Key Design Decisions

- **Result Pattern** instead of exceptions for expected business failures (e.g., invalid credentials, unauthorized access) — exceptions are reserved for truly exceptional conditions.
- **BCrypt** for password hashing — a salted, deliberately slow algorithm suited for credentials, unlike general-purpose hashes such as SHA-256.
- **JWT Bearer Authentication** with `MapInboundClaims = false` to prevent ASP.NET Core's default claim-type remapping, keeping claim names (`sub`, `email`, `role`) exactly as issued.
- **Enum-as-string** storage via EF Core's `HasConversion<string>()` for human-readable database values, paired with `JsonStringEnumConverter` so API responses also serialize enums as strings rather than integers.
- **`ActionResult<T>`** over `IActionResult` in controllers for accurate OpenAPI/Swagger schema generation.
- **`CancellationToken`** propagated through every async method, from controller to repository.
- **Service-to-service authentication via API Key** — the internal `Notifications.Api` `Create` endpoint is not meant to be called by end users, so it is protected by a custom `RequireApiKeyAttribute` (`IAsyncActionFilter`) instead of JWT, since no individual user identity is involved in that call.

## Inter-Service Communication

When a task is created or its status changes, `Tasks.Api` notifies `Notifications.Api` via a direct HTTP call:

```
Tasks.Api  →  INotificationClient (Application interface)
           →  NotificationClient (Infrastructure implementation, IHttpClientFactory Typed Client)
           →  POST /api/notifications  (with X-Api-Key header)
           →  Notifications.Api
```

This call is intentionally **best-effort**: if `Notifications.Api` is unreachable, the failure is logged but does not roll back or block the task creation, avoiding tight coupling between services.

## Authentication & Authorization Flow

1. A user registers and logs in via `Identity.Api`, receiving a signed JWT containing `sub` (user ID), `email`, and `role` claims.
2. The same JWT is presented to `Tasks.Api` and `Notifications.Api`, which validate its signature using a **shared secret key** — no direct communication with `Identity.Api` is required to authenticate a request.
3. Beyond authentication, each service enforces its own **authorization** rules in the application layer (e.g., a user may only view, update, or delete tasks and notifications they own), independent of whether the token itself is valid.

## Tech Stack

- **.NET 10** / ASP.NET Core Web API
- **Entity Framework Core** (Code-First Migrations) with **SQL Server**
- **JWT Bearer Authentication**
- **BCrypt.Net** for password hashing
- **IHttpClientFactory** for inter-service HTTP communication
- **Docker & Docker Compose** for containerized orchestration
- **Swagger / Swashbuckle** for API documentation

## Project Structure

```
TaskManagement.Microservices.sln
├── Identity.Api / Identity.Application / Identity.Domain / Identity.Infrastructure
├── Tasks.Api / Tasks.Application / Tasks.Domain / Tasks.Infrastructure
├── Notifications.Api / Notifications.Application / Notifications.Domain / Notifications.Infrastructure
└── docker-compose.yml
```

## Running with Docker (recommended)

Requires [Docker Desktop](https://www.docker.com/products/docker-desktop).

```bash
docker compose up --build
```

This builds and starts four containers on an internal Docker network: `sqlserver`, `identity.api`, `tasks.api`, and `notifications.api`. Services communicate with each other by container name (e.g., `http://notifications.api:8080`) rather than `localhost`.

| Service | URL |
|---|---|
| Identity.Api | `https://localhost:5001/swagger` |
| Tasks.Api | `https://localhost:5002/swagger` |
| Notifications.Api | `https://localhost:5003/swagger` |
| SQL Server | `localhost:1433` |

## Running Locally (without Docker)

1. Set each service's `ConnectionStrings:DefaultConnection` in `appsettings.json` to a local SQL Server instance.
2. Apply migrations for each service from the Package Manager Console:
   ```
   Update-Database -Project <Service>.Infrastructure -StartupProject <Service>.Api
   ```
3. Configure Visual Studio for **Multiple Startup Projects**, setting all three `.Api` projects to `Start`.
4. Run (F5).

## API Endpoints Summary

### Identity.Api
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Authenticate and receive a JWT |

### Tasks.Api
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/tasks` | Create a task |
| GET | `/api/tasks` | List the current user's tasks |
| GET | `/api/tasks/{id}` | Get a single task |
| PUT | `/api/tasks/{id}` | Update task details |
| PATCH | `/api/tasks/{id}/status` | Update task status only |
| DELETE | `/api/tasks/{id}` | Delete a task |

### Notifications.Api
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/notifications` | Create a notification *(internal — requires `X-Api-Key`)* |
| GET | `/api/notifications` | List the current user's notifications |
| PATCH | `/api/notifications/{id}/read` | Mark a notification as read |
| DELETE | `/api/notifications/{id}` | Delete a notification |

## Testing

All endpoints were manually tested end-to-end with Postman, covering:
- Registration, login, and JWT-protected requests
- Ownership-based authorization (users cannot access others' tasks or notifications)
- The full inter-service flow: task creation → automatic notification delivery
- Verification that the same flow works identically inside Docker containers, using container-network hostnames

## Possible Future Improvements

- Replace direct HTTP calls between services with a message broker (e.g., RabbitMQ) for asynchronous, fault-tolerant communication
- Introduce an API Gateway (e.g., YARP or Ocelot) as a single entry point
- Add centralized logging/tracing across services (e.g., Serilog + Seq, or OpenTelemetry)
- Add automated integration tests

## Author

**Antony Girgis**
[GitHub](https://github.com/antonygirgisghaly) · [LinkedIn](https://www.linkedin.com/in/antony-girgis-286876243)
