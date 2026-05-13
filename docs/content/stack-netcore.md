- [Common .NET Core Stack](#common-net-core-stack)
  - [Core Libraries](#core-libraries)
  - [Infrastructure Components](#infrastructure-components)
- [Best Practices](#best-practices)

---

# Common .NET Core Stack

Year: 2026

## Core Libraries

| Tool                          | Purpose                        | Typical Layer                 |
| ----------------------------- | ------------------------------ | ----------------------------- |
| **xUnit / NUnit**             | Unit & integration testing     | Test projects                 |
| **FluentAssertions**          | Readable assertions            | Test projects                 |
| **Moq / NSubstitute**         | Mocking dependencies           | Test projects                 |
| **MediatR**                   | Commands / Queries / Use cases | Application                   |
| **FluentValidation**          | Request validation             | Application                   |
| **AutoMapper / Mapster**      | DTO ↔ Entity mapping           | Application                   |
| **EF Core**                   | ORM / persistence              | Infrastructure                |
| **EF Core InMemory / SQLite** | Integration testing            | Test projects                 |
| **Serilog / NLog**            | Structured logging             | Cross-cutting                 |
| **Swagger / NSwag**           | API documentation              | Presentation                  |
| **OpenTelemetry**             | Tracing / telemetry            | Cross-cutting                 |
| **Redis**                     | Distributed caching            | Infrastructure                |
| **JWT / IdentityServer**      | Authentication                 | Presentation / Infrastructure |

MediatR + FluentValidation + Serilog are commonly combined in modern Clean-style .NET projects.

## Infrastructure Components

| Area                               | Common Choice                                  |
| ---------------------------------- | ---------------------------------------------- |
| **Authentication & Authorization** | IdentityServer / ASP\.NET Identity / JWT       |
| **Database**                       | EF Core + Repository + Unit of Work            |
| **Caching**                        | Redis                                          |
| **API Versioning**                 | `Microsoft.AspNetCore.Mvc.Versioning`          |
| **Observability**                  | OpenTelemetry + Microsoft Application Insights |
| **CI/CD**                          | GitHub Actions / Azure DevOps / Jenkins        |
| **Containerization**               | Docker + Kubernetes                            |

---

# Best Practices

| Practice                 | Why                                                  |
| ------------------------ | ---------------------------------------------------- |
| **Dependency Inversion** | Infrastructure depends on Application, never reverse |
| **CQRS + MediatR**       | Separate read/write responsibilities                 |
| **FluentValidation**     | Centralized validation rules                         |
| **AutoMapper / Mapster** | Reduce repetitive mapping code                       |
| **Serilog**              | Structured searchable logs                           |
| **EF Core Migrations**   | Database versioning                                  |
| **Swagger / NSwag**      | Self-documenting APIs                                |
| **Unit Tests**           | Validate Domain + Application logic                  |
| **Integration Tests**    | Validate Infrastructure behavior                     |

CQRS and MediatR are especially common in Clean and Onion implementations.
