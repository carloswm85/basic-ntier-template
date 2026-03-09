- [Dotnet Template Kit](#dotnet-template-kit)
  - [1. Available Templates](#1-available-templates)
    - [1.1 Possible Projects](#11-possible-projects)
  - [2. Available branches](#2-available-branches)
  - [3. Documentation](#3-documentation)

---

![banner](./docs/img/banner.png)

---

# Dotnet Template Kit

<https://github.com/carloswm85/dotnet-template-kit>

---

## 1. Available Templates

| Architecture                               | Status     | Description                                                                                                                                                                                                                                  | Structure Characteristics                                                                                                            | When to Use                                                             |
| ------------------------------------------ | ---------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------- |
| **N-Tier (Layered / Entire Architecture)** | Working ✅ | Classic layered architecture separating concerns into **Domain, Application, Infrastructure, and API**. Each layer depends only on the layer below it, enforcing strict boundaries and maintainability. Common in enterprise `.NET` systems. | Clear horizontal layers. Logic flows **API → Application → Domain → Infrastructure**. Strong separation of concerns and testability. | Enterprise applications, large teams, long-term maintainability.        |
| **Vertical Slice Architecture**            | Planned ⚠️ | Organizes code **by feature instead of by layer**. Each feature contains its own handlers, DTOs, validation, and logic. Often implemented with **CQRS + MediatR**. Reduces cross-project dependencies and improves modularity.               | Structure grouped by **features** (e.g., `Users`, `Orders`). Each feature encapsulates its own behavior and dependencies.            | APIs with many independent endpoints, microservices, modular monoliths. |

### 1.1 Possible Projects

| Architecture                     | Status | Description                                                                                                                                                                                                                                                     | Structure Characteristics                                                                                                                             | When to Use                                                                                                              |
| -------------------------------- | ------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------ |
| **Clean Architecture**           | ✖️     | Architecture centered on the **dependency rule**, where inner layers (Domain and Application) must not depend on outer layers (Infrastructure or UI). Popularized by Robert C. Martin. Focuses on long-term maintainability and independence from frameworks.   | Concentric layers with **Domain at the core**, surrounded by Application, Infrastructure, and Presentation. Dependencies always point inward.         | Enterprise systems, long-lived applications, teams enforcing strong architectural boundaries.                            |
| **Hexagonal (Ports & Adapters)** | ✖️     | Also known as **Ports and Adapters**, this architecture isolates business logic from external systems by defining **ports (interfaces)** and **adapters (implementations)**. The core application communicates with the outside world only through these ports. | Central application core with adapters for external systems such as databases, APIs, and message brokers. Promotes strong decoupling and testability. | Systems integrating multiple external services, applications requiring high testability and infrastructure independence. |
| **Onion Architecture**           | ✖️     | Domain-centric architecture where the **domain model sits at the center**, surrounded by layers that depend on it. Similar to Clean Architecture but more focused on domain-driven design principles.                                                           | Concentric layers: **Domain → Application Services → Infrastructure → Presentation**. Dependencies move toward the center.                            | Domain-driven systems, complex business logic, applications emphasizing domain modeling.                                 |
| **Modular Monolith**             | ✖️     | A single deployable application divided into **well-defined internal modules** with strict boundaries and communication rules. Each module encapsulates its own logic and data access.                                                                          | Codebase organized into modules (e.g., `Users`, `Billing`, `Orders`). Modules communicate through defined interfaces or events.                       | Medium to large systems that need modularity but do not require full microservices complexity.                           |
| **Minimal API Architecture**     | ✖️     | Lightweight API architecture built using `.NET` **Minimal APIs**, focusing on simplicity and minimal ceremony. Eliminates controllers in favor of direct endpoint mapping.                                                                                      | Flat structure with endpoint definitions in `Program.cs` or feature files. Uses lightweight dependency injection and minimal boilerplate.             | Microservices, prototypes, small APIs, high-performance lightweight services.                                            |
| **CQRS Architecture**            | ✖️     | Separates **command (write)** operations from **query (read)** operations, allowing each side to evolve independently and optimize performance. Often combined with event sourcing.                                                                             | Distinct models for reads and writes, separate handlers, and sometimes separate data stores. Often implemented with libraries like MediatR.           | Systems requiring scalability, complex workflows, or high read/write performance.                                        |
| **Event-Driven Architecture**    | ✖️     | System components communicate through **events** rather than direct calls. Events represent state changes and are processed asynchronously by other components.                                                                                                 | Uses message brokers or event buses. Producers publish events, consumers react to them independently.                                                 | Distributed systems, microservices, systems needing high scalability and loose coupling.                                 |

---

## 2. Available branches

| Nº  | Branch                      | Content/Stack                                                                    | Status                                       | Link                                                                                   |
| --- | --------------------------- | -------------------------------------------------------------------------------- | -------------------------------------------- | -------------------------------------------------------------------------------------- |
| A   | `main`                      | Documentation only                                                               | Reference to all branches, and documentation | [🔗](https://github.com/carloswm85/dotnet-template-kit)                                |
| B   | `dev-lightweight-netcore8`  | NET Core 8 with N-Tier architecture **+** MVC **+** Web API **+** Angular 19     | FINISHED ✅                                  | [🔗](https://github.com/carloswm85/dotnet-template-kit/tree/dev-netcore08-lightweight) |
| C   | `dev-lightweight-netcore10` | NET Core 10 with N-Tier architecture **+** MVC **+** Web API **+** Angular 20    | FINISHED ✅                                  | [🔗](https://github.com/carloswm85/dotnet-template-kit/tree/dev-netcore10-lightweight) |
| D   | `dev-identity-netcore10`    | `dev-lightweight-netcore10` **+** Identity API implementation in MVC and Web API | FINISHED ✅                                  | [🔗](https://github.com/carloswm85/dotnet-template-kit/tree/dev-netcore10-identity)    |

`dev-*` is used for stable versions.

---

## 3. Documentation

- [./docs/content/onboarding/README.md](./docs/content/onboarding/README.md)
