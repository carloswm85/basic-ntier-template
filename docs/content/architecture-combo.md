- [Architecture Combo: CA + DDD + TDA](#architecture-combo-ca--ddd--tda)
  - [Complexity Level](#complexity-level)
  - [CA](#ca)
  - [DDD](#ddd)
  - [CA + DDD](#ca--ddd)
  - [TDD with CA + DDD](#tdd-with-ca--ddd)
    - [How They Work Together](#how-they-work-together)
    - [TDD Strategy](#tdd-strategy)

---

# Architecture Combo: CA + DDD + TDA

| #     | Concept                 | In Short          | The What                                                      | The How                                                                              | The Where                                         |
| ----- | ----------------------- | ----------------- | ------------------------------------------------------------- | ------------------------------------------------------------------------------------ | ------------------------------------------------- |
| `CNA` | Clean Architecture      | Structure         | Keep business logic independent from frameworks and organized | How the solution is organized: structure, technical separation, dependency direction | Entire solution across all layers                 |
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
