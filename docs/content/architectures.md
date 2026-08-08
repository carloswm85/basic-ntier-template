- [NET Core Architectures](#net-core-architectures)
  - [Common Architectures](#common-architectures)
    - [Comparative Table](#comparative-table)
  - [Pros and Cons Table](#pros-and-cons-table)
    - [Simplified Way to Think](#simplified-way-to-think)
    - [Mental Models](#mental-models)

---

# NET Core Architectures

Warning quote:

> Started monolithic, scaled to N-Layer, then realized Vertical Slice fit our feature-driven teams way better. CQRS + MediatR came later when reporting became bottleneck. The "evolution path" is spot-on—jumping straight to microservices kills velocity. Know your domain first, evolve deliberately. The worst architecture choice isn't the pattern; it's picking one without understanding why.

## Common Architectures

| #      | Name                                          | When to Use (recommendation)                                                                                                                                                         |
| ------ | --------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `SMA`  | **Simple Monolith Architecture**              | APIs with many independent business modules, large monoliths needing clear boundaries, systems expected to evolve gradually toward distributed architectures.                        |
| `BNLA` | **Basic N-Layer Architecture**                | Small to medium projects, learning/prototyping, simple CRUD apps with limited business complexity.                                                                                   |
| `NLA`  | **N-Layer Architecture**                      | Enterprise applications, large teams, long-term maintainability. N-Layer Architecture with **Unit of Work** and **Repository** design patterns. It also includes an Angular project. |
| `VSA`  | **Vertical Slice Architecture**               | APIs with many independent endpoints, microservices, modular monoliths.                                                                                                              |
| `CNA`  | **Clean Architecture**                        | Domain-rich applications, teams prioritizing testability and strict dependency rules.                                                                                                |
| `HXA`  | **Hexagonal Architecture** (Ports & Adapters) | Systems with multiple I/O adapters (REST, CLI, messaging), high infrastructure replaceability need.                                                                                  |
| `ONA`  | **Onion Architecture**                        | DDD-aligned projects, complex domain logic needing strong layer isolation and inversion of control.                                                                                  |
| `MSA`  | **Microservices Architecture**                | Very large systems requiring independent deployment, isolated scaling, autonomous teams, and heterogeneous technology stacks.                                                        |

### Comparative Table

| Feature              | Simple Monolith Architecture                                      | N-Layer Architecture                    | Vertical Slice Architecture      | Clean Architecture                       | Hexagonal Architecture                    | Onion Architecture                     | Microservices Architecture                                                |
| -------------------- | ----------------------------------------------------------------- | --------------------------------------- | -------------------------------- | ---------------------------------------- | ----------------------------------------- | -------------------------------------- | ------------------------------------------------------------------------- |
| **Acronym**          | `SMA`                                                             | `BNLA` and `NLA`                        | `VSA`                            | `CNA`                                    | `HXA`                                     | `ONA`                                  | `MSA`                                                                     |
| **Origin**           | Domain-driven modular monolith movement (2015+)                   | Traditional enterprise systems (1990s)  | Modern .NET community (2018+)    | Uncle Bob (2012)                         | Alistair Cockburn (2005)                  | Jeffrey Palermo (2008)                 | Martin Fowler / James Lewis coined the term (2014); concept predates this |
| **Core Focus**       | Modular business boundaries inside one deployable unit            | Technical layers                        | Feature-based slices             | Domain + Use Cases                       | Ports & Adapters                          | Domain-centric layers                  | Independently deployable services                                         |
| **External Systems** | Shared infrastructure with isolated modules                       | Data layer / service layer              | Embedded per feature             | Infrastructure outer ring                | Adapters (DB, UI, API)                    | Infrastructure outer layer             | Each service owns integrations                                            |
| **Dependencies**     | Module boundaries with controlled references                      | Top → down                              | Inside each slice                | Inward                                   | Core → Ports → Adapters                   | Inward                                 | Service-to-service APIs/events                                            |
| **Best Fit**         | Large monoliths needing modularity without distributed complexity | CRUD apps, internal systems             | Medium APIs, modular monolith    | Enterprise apps, CQRS, APIs              | Microservices, multiple interfaces        | Enterprise apps, DDD-heavy systems     | Large distributed platforms                                               |
| **Readings**         | Kamil Grzybek [^3], Milan Jovanović [^4]                          | Martin Fowler [^5], Microsoft Docs [^6] | Anton Dev [^1], Julio Casal [^2] | Robert C. Martin [^7], Jason Taylor [^8] | Alistair Cockburn [^9], Jon P Smith [^10] | Jeffrey Palermo [^11], Code Maze [^12] | Martin Fowler [^13], Sam Newman [^14]                                     |

[^1]: [https://antondevtips.com/blog/vertical-slice-architecture-the-best-ways-to-structure-your-project](https://antondevtips.com/blog/vertical-slice-architecture-the-best-ways-to-structure-your-project)

[^2]: [https://juliocasal.com/blog/vertical-slice-architecture](https://juliocasal.com/blog/vertical-slice-architecture)

[^3]: [https://www.kamilgrzybek.com/design/modular-monolith-primer](https://www.kamilgrzybek.com/design/modular-monolith-primer)

[^4]: [https://www.milanjovanovic.tech/blog/modular-monolith-architecture](https://www.milanjovanovic.tech/blog/modular-monolith-architecture)

[^5]: [https://martinfowler.com/bliki/PresentationDomainDataLayering.html](https://martinfowler.com/bliki/PresentationDomainDataLayering.html)

[^6]: [https://learn.microsoft.com/aspnet/mvc/overview/older-versions-1/nerddinner/build-a-model-with-business-rule-validations](https://learn.microsoft.com/aspnet/mvc/overview/older-versions-1/nerddinner/build-a-model-with-business-rule-validations)

[^7]: [https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

[^8]: [https://jasontaylor.dev/clean-architecture-getting-started](https://jasontaylor.dev/clean-architecture-getting-started)

[^9]: [https://alistair.cockburn.us/hexagonal-architecture](https://alistair.cockburn.us/hexagonal-architecture)

[^10]: [https://www.thereformedprogrammer.net/wrapping-your-business-logic-with-anti-corruption-layers-net-core](https://www.thereformedprogrammer.net/wrapping-your-business-logic-with-anti-corruption-layers-net-core)

[^11]: [https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1)

[^12]: [https://code-maze.com/onion-architecture-in-aspnetcore](https://code-maze.com/onion-architecture-in-aspnetcore)

[^13]: [https://martinfowler.com/articles/microservices.html](https://martinfowler.com/articles/microservices.html)

[^14]: [https://samnewman.io/books/building_microservices](https://samnewman.io/books/building_microservices)

## Pros and Cons Table

| Architecture | Pros                                                                                                                                        | Cons                                                                                                                    |
| ------------ | ------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------- |
| `SMA`        | Strong modularity without distributed complexity; easier refactoring toward microservices; good team ownership; simpler deployment than MSA | Requires strict module boundaries; shared database can create coupling; horizontal scaling requires extracting services |
| `BNLA`/`NLA` | Simple to understand; clear technical separation; fast for CRUD applications                                                                | Tight coupling between layers; feature changes touch many files; harder to scale large domains                          |
| `VSA`        | Feature cohesion; localized changes; easier parallel development; good maintainability                                                      | Possible code duplication; weaker shared domain model; can become inconsistent without discipline                       |
| `CNA`        | Clear separation; framework independence; highly testable; works well with CQRS                                                             | Over-engineering risk; steeper learning curve; requires strict discipline                                               |
| `HXA`        | Flexible adapters; external systems replaceable; testable via ports; fits multiple interfaces                                               | Many abstractions; boilerplate in simple systems; governance required                                                   |
| `ONA`        | Domain isolation; strong separation of concerns; highly testable; good for rich business rules                                              | Higher setup cost; complexity for small apps; infrastructure leaks can still happen                                     |
| `MSA`        | Independent deployment; isolated scaling; team autonomy; technology freedom                                                                 | Distributed complexity; monitoring overhead; data consistency challenges; operational cost                              |

Quick reading:

- **Simple Monolith** favors **modularity without distributed complexity**
- **N-Layer** favors **simplicity**
- **Vertical Slice** favors **feature ownership**
- **Onion / Clean / Hexagonal** favor **domain isolation**
- **Microservices** favor **deployment independence**

The main tradeoff is usually:

```terminal
Simplicity  <----------------------------------------------->  Flexibility
N-Layer    Vertical Slice    Simple Monolith    Clean/Hex    Microservices
```

### Simplified Way to Think

| Architecture | Explanation                                                     |
| ------------ | --------------------------------------------------------------- |
| `SMA`        | One deployable application split into isolated business modules |
| `BNLA`/`NLA` | Organized by technical layers                                   |
| `VSA`        | Organize by business feature                                    |
| `CNA`        | Onion with **explicit use cases** (CQRS style)                  |
| `HXA`        | Focuses on **system boundaries** through ports/adapters         |
| `ONA`        | **domain-centric layering**                                     |
| `MSA`        | Organize by independent business services                       |

Clean, Onion, and Hexagonal are usually considered closely related because they all enforce **dependency inversion toward the business core**, while Vertical Slice changes **how code is organized**, and Microservices changes **deployment boundaries**.

### Mental Models

```terminal
=== `SMA`: Multiple business modules inside one application ===
Sales Module
Inventory Module
Billing Module
Shared Infrastructure

=== `BNLA`/`NLA`: Stacked technical layers ===
UI
Business
Data

=== `VSA`: Independent feature modules ===
Feature A -> full stack
Feature B -> full stack
Feature C -> full stack

=== `CNA`: Concentric layers with use-case emphasis ===
Frameworks
Adapters
Use Cases
Entities

=== `HXA`: Central core with directional ports and interchangeable adapters ===
Driving Adapter -> Driving Port -> Core -> Driven Port -> Driven Adapter

=== `ONA`: Concentric layers around domain ===
Infrastructure
Application
Domain

=== `MSA`: Distributed autonomous services ===
Auth Service
Orders Service
Billing Service
Notification Service
```
