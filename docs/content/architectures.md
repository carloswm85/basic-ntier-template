- [NET Core Architecture](#net-core-architecture)
  - [Common Architectures](#common-architectures)
    - [Descriptive Table](#descriptive-table)
  - [Pros and Cons Table](#pros-and-cons-table)
    - [Simplified Way to Think](#simplified-way-to-think)
    - [Mental Models](#mental-models)
- [Architecture Combo: CA + DDD + TDA](#architecture-combo-ca--ddd--tda)
  - [Complexity Level](#complexity-level)
  - [CA](#ca)
  - [DDD](#ddd)
  - [CA + DDD](#ca--ddd)
  - [TDD with CA + DDD](#tdd-with-ca--ddd)
    - [How They Work Together](#how-they-work-together)
    - [TDD Strategy](#tdd-strategy)
- [Common .NET Core Stack](#common-net-core-stack)
  - [Core Libraries](#core-libraries)
  - [Infrastructure Components](#infrastructure-components)
- [Best Practices](#best-practices)
- [Per Architecture](#per-architecture)

---

# NET Core Architecture

Warning quote:

> Started monolithic, scaled to N-Layer, then realized Vertical Slice fit our feature-driven teams way better. CQRS + MediatR came later when reporting became bottleneck. The "evolution path" is spot-on—jumping straight to microservices kills velocity. Know your domain first, evolve deliberately. The worst architecture choice isn't the pattern; it's picking one without understanding why.

## Common Architectures

### Descriptive Table

| Feature              | N-Layer Architecture                    | Vertical Slice Architecture      | Onion Architecture                 | Clean Architecture          | Hexagonal Architecture             | Microservices Architecture                 |
| -------------------- | -------------------------------------- | -------------------------------- | ---------------------------------- | --------------------------- | ---------------------------------- | ------------------------------------------ |
| **#**                | (a)                                    | (b)                              | (c)                                | (d)                         | (e)                                | (f)                                        |
| **Achronym**         | `NTA`                                  | `VSA`                            | `ONA`                              | `CA`                        | `HXA`                              | `MSA`                                      |
| **Origin**           | Traditional enterprise systems (1990s) | Modern .NET community (2018+)    | Jeffrey Palermo (2008)             | Uncle Bob (2012)            | Alistair Cockburn (2005)           | Martin Fowler / industry evolution (2014+) |
| **Core Focus**       | Technical layers                       | Feature-based slices             | Domain-centric layers              | Domain + Use Cases          | Ports & Adapters                   | Independently deployable services          |
| **External Systems** | Data layer / service layer             | Embedded per feature             | Infrastructure outer layer         | Infrastructure outer ring   | Adapters (DB, UI, API)             | Each service owns integrations             |
| **Dependencies**     | Top → down                             | Inside each slice                | Inward                             | Inward                      | Core → Ports → Adapters            | Service-to-service APIs/events             |
| **Best Fit**         | CRUD apps, internal systems            | Medium APIs, modular monoliths   | Enterprise apps, DDD-heavy systems | Enterprise apps, CQRS, APIs | Microservices, multiple interfaces | Large distributed platforms                |
| **Readings**         | TODO                                   | Anton Dev [^1], Julio Casal [^2] | TODO                               | TODO                        | TODO                               | TODO                                       |

[^1]: <https://antondevtips.com/blog/vertical-slice-architecture-the-best-ways-to-structure-your-project>

[^2]: <https://juliocasal.com/blog/vertical-slice-architecture>

## Pros and Cons Table

| #   | Architecture       | Pros                                                                                           | Cons                                                                                              |
| --- | ------------------ | ---------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- |
| (a) | **N-Layer**         | Simple to understand; clear technical separation; fast for CRUD applications                   | Tight coupling between layers; feature changes touch many files; harder to scale large domains    |
| (b) | **Vertical Slice** | Feature cohesion; localized changes; easier parallel development; good maintainability         | Possible code duplication; weaker shared domain model; can become inconsistent without discipline |
| (c) | **Onion**          | Domain isolation; strong separation of concerns; highly testable; good for rich business rules | Higher setup cost; complexity for small apps; infrastructure leaks can still happen               |
| (d) | **Clean**          | Clear separation; framework independence; highly testable; works well with CQRS                | Over-engineering risk; steeper learning curve; requires strict discipline                         |
| (e) | **Hexagonal**      | Flexible adapters; external systems replaceable; testable via ports; fits multiple interfaces  | Many abstractions; boilerplate in simple systems; governance required                             |
| (f) | **Microservices**  | Independent deployment; isolated scaling; team autonomy; technology freedom                    | Distributed complexity; monitoring overhead; data consistency challenges; operational cost        |

Quick reading:

- **N-Layer** favors **simplicity**
- **Vertical Slice** favors **feature ownership**
- **Onion / Clean / Hexagonal** favor **domain isolation**
- **Microservices** favor **deployment independence**

The main tradeoff is usually:

```text id="pk4r2m"
Simplicity  <---------------------------->  Flexibility
N-Layer      Vertical      Clean/Hex       Microservices
```

### Simplified Way to Think

| #   | Architecture                    | Explanation                                             |
| --- | ------------------------------- | ------------------------------------------------------- |
| (a) | **N-Layer Architecture**         | Organized by technical layers                           |
| (b) | **Vertical Slice Architecture** | Organize by business feature                            |
| (c) | **Onion Architecture**          | **domain-centric layering**                             |
| (d) | **Clean Architecture**          | Onion with **explicit use cases** (CQRS style)          |
| (e) | **Hexagonal Architecture**      | Focuses on **system boundaries** through ports/adapters |
| (f) | **Microservices Architecture**  | Organize by independent business services               |

Clean, Onion, and Hexagonal are usually considered closely related because they all enforce **dependency inversion toward the business core**, while Vertical Slice changes **how code is organized**, and Microservices changes **deployment boundaries**. ([TAKT R&D][1])

### Mental Models

```terminal
=== (a) N-Layer: Stacked technical layers ===
UI
Business
Data

=== (b) Vertical Slice: Independent feature modules ===
Feature A -> full stack
Feature B -> full stack
Feature C -> full stack

=== (c) Onion: Concentric layers around domain ===
Infrastructure
Application
Domain

=== (d) Clean: Concentric layers with use-case emphasis ===
Frameworks
Adapters
Use Cases
Entities

=== (e) Hexagonal: Central core with interchangeable adapters ===
Adapter <- Port -> Core <- Port -> Adapter

=== (f) Microservices: Distributed autonomous services ===
Auth Service
Orders Service
Billing Service
Notification Service
```

---

# Architecture Combo: CA + DDD + TDA

| #     | Concept                 | In Short          | The What                                                      | The How                                                                              | The Where                                         |
| ----- | ----------------------- | ----------------- | ------------------------------------------------------------- | ------------------------------------------------------------------------------------ | ------------------------------------------------- |
| `CA`  | Clean Architecture      | Structure         | Keep business logic independent from frameworks and organized | How the solution is organized: structure, technical separation, dependency direction | Entire solution across all layers                 |
| `DDD` | Domain Driven Design    | Business Model    | Represent complex business rules correctly                    | How the business logic is modeled: behavior, business correctness, rich domain       | Inside CA Domain layer                            |
| `TDD` | Test Driven Development | Design validation | Validate behaviour and evolve design through tests            | Drives implementation through red → green → refactor cycles                          | Across every layer, mainly Domain and Application |

> TDD + Clean Architecture + DDD = highly maintainable business systems

- Clean Architecture defines the architectural boundaries, DDD defines the business language and model, and TDD validates the design continuously by making each layer testable.

## Complexity Level

| Situation              | Recommendation          |
| ---------------------- | ----------------------- |
| Simple CRUD app        | Clean Architecture only |
| Complex business rules | Add DDD                 |
| Enterprise system      | Clean + DDD             |
| Microservices          | Strong fit              |

## CA

| #   | Layer          | Responsibility   | Example in .NET                                         |
| --- | -------------- | ---------------- | ------------------------------------------------------- |
| `A` | Domain         | Business rules   | Entities, Value Objects                                 |
| `B` | Application    | Use cases        | Commands and queries (CQRS handlers), Services, MediatR |
| `C` | Infrastructure | External systems | EF Core, Email, File system                             |
| `D` | Presentation   | API / UI         | ASP\.NET Core Controllers                               |

## DDD

DDD focuses on the business language and domain behavior.

| DDD Element    | Purpose                      | Example            |
| -------------- | ---------------------------- | ------------------ |
| Entity         | Object with identity         | Order              |
| Value Object   | Immutable descriptive object | Money              |
| Aggregate      | Consistency boundary         | Order + OrderItems |
| Repository     | Abstract persistence         | IOrderRepository   |
| Domain Service | Domain logic across entities | PricingService     |
| Domain Event   | Business event               | OrderPlacedEvent   |

## CA + DDD

| #   | Clean Layer    | DDD Content                            |
| --- | -------------- | -------------------------------------- |
| `A` | Domain         | Entities, Value Objects, Domain Events |
| `B` | Application    | Use Cases, DTOs                        |
| `C` | Infrastructure | Repositories implementation            |
| `D` | Presentation   | API endpoints                          |

## TDD with CA + DDD

| Step     | Meaning               |
| -------- | --------------------- |
| Red      | Write failing test    |
| Green    | Write minimal code    |
| Refactor | Improve design safely |

- Clean Architecture makes TDD easier because:
  - Dependencies are abstracted.
  - Business logic is isolated.
  - Infrastructure can be mocked.
  - Tests run fast.
- TDD helps verify these rules encouraged by DDD:
  - Rich entities
  - Business invariants
  - Ubiquitous language

### How They Work Together

| Combination        | Purpose                                                |
| ------------------ | ------------------------------------------------------ |
| **CA + DDD**       | Keeps business rules isolated and meaningful           |
| **CA + TDD**       | Makes each layer independently testable                |
| **DDD + TDD**      | Helps refine domain behavior safely                    |
| **CA + DDD + TDD** | Produces maintainable and evolvable enterprise systems |

### TDD Strategy

| #   | Layer          | What to Test     | TDD Suitability |
| --- | -------------- | ---------------- | --------------- |
| `A` | Domain         | Business rules   | Excellent       |
| `B` | Application    | Commands/Queries | Excellent       |
| `C` | Infrastructure | EF Core, APIs    | Partial         |
| `D` | UI/API         | Endpoints        | Optional        |

- TDD is most useful for:
  - Domain logic
  - Use cases
  - Validation rules

---

# Common .NET Core Stack

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

---

# Per Architecture

| #   | Architecture       | Common .NET Stack                                              |
| --- | ------------------ | -------------------------------------------------------------- |
| (a) | **N-Layer**         | ASP\.NET MVC + EF Core + Services                              |
| (b) | **Vertical Slice** | ASP\.NET Core + MediatR + Minimal APIs                         |
| (c) | **Onion**          | ASP\.NET Core + EF Core + Repository + CQRS + MediatR          |
| (d) | **Clean**          | ASP\.NET Core + MediatR + FluentValidation + EF Core + Serilog |
| (e) | **Hexagonal**      | ASP\.NET Core + Ports/Adapters + EF Core + Messaging           |
| (f) | **Microservices**  | ASP\.NET Core + Docker + Kubernetes + Messaging                |

Onion, Clean, and Hexagonal often share a very similar technical stack in .NET because they all isolate the business core from infrastructure.
