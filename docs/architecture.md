# Codex — Software Architecture Documentation

> **Codex** is a paid publishing platform built on .NET 10 with ASP.NET Core. It allows authors to write and publish posts, organized by categories, which readers can comment on.

---

## Table of Contents

1. [High-Level Overview](#1-high-level-overview)
2. [Solution Structure](#2-solution-structure)
3. [Architectural Style](#3-architectural-style)
4. [Layer Breakdown](#4-layer-breakdown)
   - [Domain Layer](#41-domain-layer-codexdomain)
   - [Application Layer](#42-application-layer-codexapplication)
   - [Infrastructure Layer](#43-infrastructure-layer-codexinfrastructure)
   - [API Layer](#44-api-layer-codexapi)
   - [Host / Aspire](#45-host--aspire)
5. [Domain Model](#5-domain-model)
6. [Key Patterns & Techniques](#6-key-patterns--techniques)
   - [Result Pattern](#61-result-pattern)
   - [CQRS with MediatR](#62-cqrs-with-mediatr)
   - [Repository & Unit of Work](#63-repository--unit-of-work)
   - [Domain Events](#64-domain-events)
   - [Minimal API Endpoint Abstraction](#65-minimal-api-endpoint-abstraction)
   - [Request Validation (FluentValidation)](#66-request-validation-fluentvalidation)
   - [Error Handling & Problem Details](#67-error-handling--problem-details)
   - [DTOs & Mappers](#68-dtos--mappers)
   - [Pagination](#69-pagination)
   - [Module Registration Pattern](#610-module-registration-pattern)
7. [Technology Stack](#7-technology-stack)
8. [Database](#8-database)
9. [Observability](#9-observability)
10. [Testing Strategy](#10-testing-strategy)
11. [Dependency Flow](#11-dependency-flow)
12. [Conventions & Design Decisions](#12-conventions--design-decisions)

---

## 1. High-Level Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                        .NET Aspire AppHost                       │
│   ┌───────────────────────────────────────────────────────────┐  │
│   │                     Codex.Api (HTTP)                      │  │
│   │  Minimal API · FluentValidation · Scalar Docs · OpenAPI   │  │
│   └──────────────────────────┬────────────────────────────────┘  │
│                               │ MediatR (ISender)                 │
│   ┌───────────────────────────▼────────────────────────────────┐  │
│   │               Codex.Application                            │  │
│   │   Commands · Queries · DTOs · Repository Interfaces        │  │
│   └──────────┬─────────────────────────┬───────────────────────┘  │
│              │ Domain model            │ Repository interfaces     │
│   ┌──────────▼──────────┐   ┌──────────▼───────────────────────┐  │
│   │   Codex.Domain      │   │     Codex.Infrastructure          │  │
│   │ Entities · Events   │   │ EF Core · Npgsql · Repositories   │  │
│   │ Errors · Results    │   │ Migrations · Entity Config        │  │
│   └─────────────────────┘   └───────────────────────────────────┘  │
│                                        │                            │
│                               ┌────────▼────────┐                  │
│                               │   PostgreSQL     │                  │
│                               └─────────────────┘                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 2. Solution Structure

```
Codex/
├── Codex.slnx                        # Solution file
├── docs/
│   ├── todo.md
│   └── architecture.md               # This file
│
├── host/
│   ├── Codex.AppHost/                # .NET Aspire orchestration host
│   └── Codex.ServiceDefaults/        # Shared Aspire service defaults
│
├── src/
│   ├── Codex.Api/                    # Presentation layer (Minimal API)
│   ├── Codex.Application/            # Application layer (CQRS, DTOs)
│   ├── Codex.Domain/                 # Domain layer (Entities, Rules)
│   └── Codex.Infrastructure/         # Infrastructure layer (EF Core, DB)
│
└── test/
    ├── Codex.Domain.UnitTests/        # Domain entity unit tests
    ├── Codex.Application.UnitTests/   # Application handler unit tests
    ├── Codex.ArchitectureTests/       # Layer dependency rule tests
    └── Codex.Tests/                   # Shared test utilities / builders
```

---

## 3. Architectural Style

Codex follows **Clean Architecture** (also known as Onion Architecture). The core principle is that:

- **Domain** has zero external dependencies — it is the innermost layer.
- **Application** depends only on Domain (defines repository interfaces, orchestrates use-cases).
- **Infrastructure** depends on Application (implements the repository interfaces).
- **API** depends on Application (sends commands/queries via MediatR) and Infrastructure (registered via DI).

This makes domain logic completely isolated, independently testable, and free from framework coupling.

```
         ┌──────────────────────────┐
         │        Domain            │  ← No dependencies
         │  (Entities, Rules,       │
         │   Errors, Events)        │
         └──────────┬───────────────┘
                    │
         ┌──────────▼───────────────┐
         │       Application        │  ← Depends on: Domain
         │  (Commands, Queries,     │
         │   DTOs, Repo Interfaces) │
         └──────────┬───────────────┘
                    │
         ┌──────────▼───────────────┐    ┌──────────────────────────┐
         │     Infrastructure       │    │          API             │
         │  (EF Core, Repositories, │    │  (Endpoints, Validators, │
         │   Migrations, Npgsql)    │    │   Exception Handlers)    │
         └──────────────────────────┘    └──────────────────────────┘
```

Architecture integrity is **automatically enforced** by `Codex.ArchitectureTests` using `NetArchTest.Rules`.

---

## 4. Layer Breakdown

### 4.1 Domain Layer (`Codex.Domain`)

The heart of the system. Contains all business rules and domain concepts. Has **no dependency on any framework** besides `MediatR.Contracts` (for `INotification` on domain events).

| Folder         | Contents |
|----------------|---------- |
| `Entities/`    | `Author`, `Post`, `Category`, `Comment`, `Reader`, base `Entity` |
| `Entities/Base/` | Abstract `Entity` base class |
| `Enumerations/` | `PostStatus` (`Draft`, `Published`) |
| `Errors/`      | Static error factories per aggregate (`PostErrors`, `AuthorErrors`, etc.) |
| `Events/`      | `IDomainEvent`, concrete domain event records |
| `Outcomes/`    | `Result`, `Result<T>`, `Error`, `ErrorType` |

**Key design choices:**
- All entities have **private constructors** and expose a static `Create(...)` factory method.
- Constructors are called only from factories, keeping the entity in a valid state from birth.
- All mutation methods return `Result` or `Result<T>`, indicating success or failure.
- Collections are exposed as `IReadOnlyList<T>` — internal backing lists are `private readonly`.
- IDs are **UUIDv7** (`Guid.CreateVersion7()`), which are time-sortable and database index-friendly.

---

### 4.2 Application Layer (`Codex.Application`)

Orchestrates use-cases. No EF Core, no HTTP, no framework details.

| Folder       | Contents |
|--------------|----------|
| `Commands/`  | CQRS command records + their handlers (per aggregate) |
| `Queries/`   | CQRS query records + their handlers (per aggregate) |
| `Mediator/`  | `ICommand`, `ICommand<T>`, `ICommandHandler`, `IQuery<T>`, `IQueryHandler` interfaces |
| `Data/`      | Repository interfaces (`IPostRepository`, `IUnitOfWork`, etc.) |
| `Dtos/`      | Response DTO records (`PostDto`, `AuthorDto`, etc.) |
| `Dtos/Base/` | `EntityDto` base record |
| `Dtos/Mappers/` | Static extension mappers (using C# 14 `extension` blocks) |
| `Dtos/Pagination/` | `PaginationDto<T>`, `PaginationQueryDto` |

**Visibility:** All command/query/handler types are `internal` — only the command/query records themselves are `public`. `InternalsVisibleTo` is used to allow test projects access.

---

### 4.3 Infrastructure Layer (`Codex.Infrastructure`)

Implements application interfaces using EF Core + PostgreSQL.

| Folder                        | Contents |
|-------------------------------|----------|
| `EfCore/`                     | `ApplicationDbContext` (implements `IUnitOfWork`) |
| `EfCore/Repositories/`        | `PostEfCoreRepository`, `AuthorEfCoreRepository`, etc. |
| `EfCore/EntityTypeConfiguration/` | Fluent API configurations per entity |
| `EfCore/Migrations/`          | EF Core migration history |

- `ApplicationDbContext` applies all entity configurations automatically via `ApplyConfigurationsFromAssembly`.
- `ApplicationDbContextFactory` provides design-time context creation for EF Core tooling (`dotnet ef migrations add`).
- Migrations are applied automatically at startup via `app.ApplyMigrations()`.

---

### 4.4 API Layer (`Codex.Api`)

Handles HTTP concerns. Uses **ASP.NET Core Minimal APIs**.

| Folder          | Contents |
|-----------------|----------|
| `Endpoints/`    | One class per endpoint, grouped by aggregate (`Posts/`, `Authors/`, etc.) |
| `Exceptions/`   | `GlobalExceptionHandler`, `RequestValidationExceptionHandler` |
| `Extensions/`   | `EndpointExtensions` (auto-discovery), `ResultExtensions` (Result → ProblemDetails), `DatabaseExtensions` (migration helper) |
| `Program.cs`    | Application bootstrap |

---

### 4.5 Host / Aspire

**`Codex.AppHost`** — .NET Aspire orchestration host. Declares and connects all infrastructure resources:
- Provisions a **PostgreSQL** container with a persistent data volume.
- Registers the `Codex.Api` project and wires it to the database.
- Exposes a health check endpoint at `/health`.

**`Codex.ServiceDefaults`** — Shared Aspire defaults injected into every service:
- OpenTelemetry (traces, metrics, logs)
- Service Discovery
- HTTP resilience (standard resilience pipeline)
- Health checks (`/health`, `/alive`)

---

## 5. Domain Model

### Entities

| Entity     | Properties | Relationships |
|------------|------------|---------------|
| `Author`   | `FirstName`, `LastName`, `Biography` | Has many `Post` |
| `Post`     | `Title`, `Content`, `Status`, `PublishedAtUtc` | Belongs to `Author`, belongs to `Category`, has many `Comment` |
| `Category` | `Name` | Has many `Post` |
| `Comment`  | `Content` | Belongs to `Post`, belongs to `Reader` |
| `Reader`   | `FirstName`, `LastName` | Has many `Comment` |

### Base Entity

```
Entity (abstract)
  ├── Id: Guid (UUIDv7, set once)
  ├── CreatedAtUtc: DateTimeOffset
  ├── UpdatedAtUtc: DateTimeOffset? (nullable)
  ├── GetDomainEvents(): IReadOnlyList<IDomainEvent>
  ├── ClearDomainEvents()
  └── RaiseDomainEvent(IDomainEvent) [protected]
```

### Post Lifecycle

```
Create() → Status: Draft
               │
           Publish() → Status: Published
                            └── PublishedAtUtc is set
```

Only `Draft` posts can be published (enforced by domain rule, returns `Error.Conflict` otherwise).

---

## 6. Key Patterns & Techniques

### 6.1 Result Pattern

A custom Railway-Oriented Programming / Result pattern is implemented in `Codex.Domain.Outcomes`:

```
Result
  ├── IsSuccess: bool
  ├── IsFailure: bool
  └── Error: Error (code, description, ErrorType)

Result<TValue> : Result
  └── Value: TValue (throws if failure)
```

**Error types:**
| `ErrorType`         | HTTP Mapping        |
|---------------------|---------------------|
| `Failure`           | 500 Internal Server Error |
| `RequestValidation` | 400 Bad Request     |
| `Validation`        | 400 Bad Request     |
| `NotFound`          | 404 Not Found       |
| `Conflict`          | 409 Conflict        |

The `ResultExtensions.ToProblemDetails()` method converts any failing `Result` into an RFC 7807-compliant `ProblemDetails` response.

---

### 6.2 CQRS with MediatR

Commands and queries are separated from the beginning:

```
ICommand            → IRequest<Result>
ICommand<TResponse> → IRequest<Result<TResponse>>
IQuery<TResponse>   → IRequest<Result<TResponse>>
```

Each has a corresponding handler interface that wraps MediatR's `IRequestHandler`:

```
ICommandHandler<TCommand>           : IRequestHandler<TCommand, Result>
ICommandHandler<TCommand, TResponse>: IRequestHandler<TCommand, Result<TResponse>>
IQueryHandler<TQuery, TResponse>    : IRequestHandler<TQuery, Result<TResponse>>
```

All commands and queries are **records** — immutable, value-equality by default.

Commands and queries are dispatched from its endpoint via `ISender.Send(...)`. MediatR is registered in `ApplicationModule` and auto-discovers all handlers via assembly scanning.

---

### 6.3 Repository & Unit of Work

The Application layer defines repository interfaces in `Codex.Application.Data`:

```
IBaseRepository (marker)
  ├── IAuthorRepository
  ├── ICategoryRepository
  ├── ICommentRepository
  ├── IPostRepository
  └── IReaderRepository

IUnitOfWork
  └── SaveChangesAsync(CancellationToken): Task<int>
```

Infrastructure provides EF Core implementations (`AuthorEfCoreRepository`, etc.). `ApplicationDbContext` implements `IUnitOfWork` directly.

All repositories are registered as **scoped** services. The Unit of Work is committed explicitly after all repository mutations in a handler.

---

### 6.4 Domain Events

Domain events are defined in `Codex.Domain.Events` and implement `IDomainEvent : INotification` (MediatR contract).

Events are raised inside entity methods using `RaiseDomainEvent(...)` and accumulated in the base `Entity` class. They can be dispatched after `SaveChangesAsync` as part of an outbox or direct dispatch pattern (planned for future).

**Example:** `Category.Create()` raises `CategoryCreatedDomainEvent(CategoryId)`.

---

### 6.5 Minimal API Endpoint Abstraction

Each endpoint implements the `IEndpoint` interface:

```csharp
internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
```

`EndpointExtensions` auto-discovers all `IEndpoint` implementations in the executing assembly via reflection and registers them, then maps them during startup. This keeps `Program.cs` clean and makes each endpoint fully self-contained.

Each endpoint class typically contains:
- `Request` record (inner class)
- `Response` record (inner class)
- `MapEndpoint(...)` method
- `Handler(...)` static method
- `RequestValidator` inner class (FluentValidation)

---

### 6.6 Request Validation (FluentValidation)

`FluentValidation` is used for HTTP request validation. Each endpoint defines its own `RequestValidator : AbstractValidator<Request>` as a nested class.

Validators are registered automatically via `AddValidatorsFromAssemblyContaining<Program>()`.

In handlers, `validator.ValidateAndThrowAsync(...)` throws a `ValidationException` on failure, which is caught by `RequestValidationExceptionHandler` and transformed into a `400 Bad Request` ProblemDetails response.

---

### 6.7 Error Handling & Problem Details

Two exception handlers are registered in order:

| Handler | Catches | Returns |
|---------|---------|---------|
| `RequestValidationExceptionHandler` | `FluentValidation.ValidationException` | `400 Bad Request` with error details |
| `GlobalExceptionHandler` | Any unhandled `Exception` | `500 Internal Server Error` |

Both use `IProblemDetailsService` to write RFC 7807-compliant JSON responses.

Domain-level failures (from `Result.IsFailure`) are converted at the endpoint level via `result.ToProblemDetails()` — no exceptions involved.

---

### 6.8 DTOs & Mappers

DTOs are **records** defined in `Codex.Application.Dtos`. They are used as output models for queries and command responses — never directly exposed as entity types.

Mapping from entity to DTO is done via **static extension methods** using C# 14's `extension` feature (type-based extension blocks):

```csharp
// PostMapper.cs
extension(Post post)
{
    internal PostDto ToPostDto() => new(...);
}
```

This keeps mapping logic co-located with each type, without external mapping libraries.

---

### 6.9 Pagination

Paginated list responses use `PaginationDto<T>`:

```
PaginationDto<T>
  ├── Items: IReadOnlyList<T>
  ├── PageNumber: int
  ├── PageSize: int
  ├── TotalCount: int
  ├── TotalPages: int (computed)
  ├── HasPreviousPage: bool (computed)
  └── HasNextPage: bool (computed)
```

Paginated queries extend `PaginationQueryDto(PageNumber, PageSize)`.

---

### 6.10 Module Registration Pattern

Instead of deeply nested DI registration in `Program.cs`, each layer exposes a static extension method on `IServiceCollection`:

```csharp
services.AddApplicationModule(configuration);    // Registers MediatR
services.AddInfrastructureModule(configuration); // Registers EF Core + Repositories
```

This keeps bootstrap code clean and each layer responsible for registering its own services.

---

## 7. Technology Stack

| Category | Technology | Version |
|----------|-----------|---------|
| Runtime | **.NET 10** | 10.0 |
| Web Framework | **ASP.NET Core Minimal API** | 10.0 |
| ORM | **Entity Framework Core** | 10.0 |
| Database Driver | **Npgsql (EF Core PostgreSQL)** | 10.0 |
| Mediator | **MediatR** | 14.1 |
| Validation | **FluentValidation** | 12.1 |
| API Docs | **Scalar** + **Microsoft.AspNetCore.OpenApi** | 2.x / 10.0 |
| Orchestration | **.NET Aspire** | 13.2 |
| Observability | **OpenTelemetry** (traces, metrics, logs) | 1.15 |
| Tracing (DB) | **Npgsql.OpenTelemetry** | 10.0 |
| HTTP Resilience | **Microsoft.Extensions.Http.Resilience** | 10.2 |
| Service Discovery | **Microsoft.Extensions.ServiceDiscovery** | 10.2 |
| Test Framework | **xUnit** | 2.9 |
| Assertions | **Shouldly** | 4.3 |
| Mocking | **NSubstitute** | 5.3 |
| Architecture Tests | **NetArchTest.Rules** | 1.3 |
| Coverage | **coverlet.collector** | 6.0 |

---

## 8. Database

- **Engine:** PostgreSQL (provisioned by .NET Aspire via Docker container)
- **ORM:** Entity Framework Core with Npgsql provider
- **Schema management:** Code-first migrations (`EfCore/Migrations/`)
- **Migrations applied:** Automatically at startup via `dbContext.Database.Migrate()`
- **Entity configuration:** Fluent API in separate `IEntityTypeConfiguration<T>` classes, loaded via `ApplyConfigurationsFromAssembly`
- **Primary keys:** UUIDv7 GUIDs (`Guid.CreateVersion7()`) — application-generated, time-sortable
- **Connection string name:** `codex-db` (provided by Aspire to the API project)

### Configured Tables

| Table      | Notes |
|------------|-------|
| `Authors`  | |
| `Categories` | |
| `Posts`    | FK → Authors, FK → Categories |
| `Comments` | FK → Posts, FK → Readers |
| `Readers`  | |

---

## 9. Observability

Configured in `Codex.ServiceDefaults` and applied to all Aspire-managed services:

| Signal | Details |
|--------|---------|
| **Traces** | ASP.NET Core request tracing, HTTP client tracing, Npgsql DB tracing. Health check requests excluded. |
| **Metrics** | ASP.NET Core metrics, HTTP client metrics, .NET runtime metrics |
| **Logs** | OpenTelemetry logging with formatted messages and scopes |
| **Export** | OTLP exporter (configured via `OTEL_EXPORTER_OTLP_ENDPOINT` environment variable) |
| **Health Checks** | `/health` (readiness), `/alive` (liveness) — available in Development |

---

## 10. Testing Strategy

The solution follows a multi-layer testing approach:

### 10.1 Domain Unit Tests (`Codex.Domain.UnitTests`)

Tests pure domain logic — entity creation, business rule enforcement, domain events. No mocks needed; the domain has no external dependencies.

**Tests:** Entities, Enumerations, Errors, Events, Outcomes

### 10.2 Application Unit Tests (`Codex.Application.UnitTests`)

Tests command and query handlers with repository interfaces mocked using **NSubstitute**. Tests the orchestration logic in isolation.

**Tests:** Command handlers, Query handlers, Mappers

### 10.3 Architecture Tests (`Codex.ArchitectureTests`)

Uses **NetArchTest.Rules** to enforce Clean Architecture dependency rules:

| Rule |
|------|
| Domain layer must NOT depend on Application layer |
| Domain layer must NOT depend on Infrastructure layer |
| Application layer must NOT depend on Infrastructure layer |

Also validates structural rules about Commands, Queries, DTOs, Mediator interfaces, and Domain types.

### 10.4 Shared Test Utilities (`Codex.Tests`)

Common test fixtures, object builders, and helpers shared across test projects.

---

## 11. Dependency Flow

```
Codex.Api
  │
  ├── depends on → Codex.ServiceDefaults (Aspire service defaults)
  ├── depends on → Codex.Infrastructure  (registers DI modules)
  │                  └── depends on → Codex.Application
  │                                      └── depends on → Codex.Domain
  │
  └── (Codex.Api also transitively uses Codex.Application via MediatR)
```

```
NuGet dependency highlights:
  Codex.Domain         ← MediatR.Contracts (INotification only)
  Codex.Application    ← MediatR (ISender, IRequestHandler)
  Codex.Infrastructure ← Npgsql.EntityFrameworkCore.PostgreSQL
  Codex.Api            ← FluentValidation, Scalar, Microsoft.AspNetCore.OpenApi
  Codex.AppHost        ← Aspire.Hosting.PostgreSQL
  Codex.ServiceDefaults← OpenTelemetry stack, ServiceDiscovery, Http.Resilience
```

---

## 12. Conventions & Design Decisions

| Convention | Details |
|------------|---------|
| **Sealed classes** | Virtually all concrete types are `sealed` to prevent unintended inheritance |
| **Private constructors** | Entities use private constructors; creation flows through static `Create(...)` factory methods |
| **Records for immutable data** | Commands, queries, DTOs, and domain events are C# `record` types |
| **Internal by default** | Handlers, mappers, validators, and repository implementations are `internal`; public API surface is minimal |
| **IReadOnlyList for collections** | Entity collections are exposed as `IReadOnlyList<T>` — no external mutation |
| **Result instead of exceptions** | All expected failures are represented as `Result.Failure(error)`, not exceptions |
| **UUIDv7 IDs** | Time-sortable, application-generated GUIDs — no round-trips to DB for ID generation |
| **Nullable enabled** | All projects have `<Nullable>enable</Nullable>` |
| **InternalsVisibleTo** | Test projects access internal types through `InternalsVisibleTo` attributes in `.csproj` |
| **Module pattern** | Each layer owns its own DI registration via extension methods |
| **Extension-based mappers** | C# 14 `extension` blocks for mapping instead of AutoMapper or Mapster |
| **Dependency direction** | Enforced via architecture tests; Inner layers never reference outer layers |

