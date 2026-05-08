- [DotNet Template Kit](#dotnet-template-kit)
  - [1. Available Templates](#1-available-templates)
    - [1.1 Recommended Learning Path](#11-recommended-learning-path)
  - [2. Available Branches](#2-available-branches)
  - [3. Additional Documentation](#3-additional-documentation)
    - [Repository Documentation](#repository-documentation)
    - [Online](#online)

---

![banner](./docs/img/banner.png)

---

# DotNet Template Kit

<https://github.com/carloswm85/dotnet-template-kit>

**Introduction**:

- Dot Net solution templates for different architectures.

---

## 1. Available Templates

| #      | Name                                          | Status | When to Use (recommendation)                                                                        | Docs                                                                     |
| ------ | --------------------------------------------- | ------ | --------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------ |
| `BNLA` | **Basic N-Layer Architecture**                | 🟡     | Small to medium projects, learning/prototyping, simple CRUD apps with limited business complexity.  | [README.md](./templates/BasicNLayerTemplate/README.md)                   |
| `CNA`  | **Clean Architecture**                        | 🟠     | Domain-rich applications, teams prioritizing testability and strict dependency rules.               | [README.md](./templates/CleanArchitectureTemplate/README.md)             |
| `CNLA` | **Complex N-Layer Architecture**              | 🟢     | Enterprise applications, large teams, long-term maintainability.                                    | [README.md](./templates/ComplexNLayerTemplate/README.md)                 |
| `HGA`  | **Hexagonal Architecture** (Ports & Adapters) | 🟠     | Systems with multiple I/O adapters (REST, CLI, messaging), high infrastructure replaceability need. | [README.md](./templates/HexagonalArchitectureTemplate/README.md)         |
| `MMA`  | **Modular Monolithic Architecture**           | 🟡     | APIs with many independent endpoints, microservices, modular monoliths.                             | [README.md](./templates/ModularMonolithicArchitectureTemplate/README.md) |
| `ONA`  | **Onion Architecture**                        | 🟠     | DDD-aligned projects, complex domain logic needing strong layer isolation and inversion of control. | [README.md](./templates/OnionArchitectureTemplate/README.md)             |
| `VSA`  | **Vertical Slice Architecture**               | 🟡     | APIs with many independent endpoints, microservices, modular monoliths.                             | [README.md](./templates/VerticalSliceTemplate/README.md)                 |

🟠 = Planned
🟡 = In Progress
🟢 = Finished
🔴 = Blocked/Unfinished

### 1.1 Recommended Learning Path

| #   | Architecture | Wikipedia |
| --- | ------------ | --------- |
| 1   | `MMA`        |           |
| 2   | `BNLA`       |           |
| 3   | `CNLA`       |           |
| 4   | `VSA`        |           |

---

## 2. Available Branches

| Branch                     | Content/Stack                                                 | Status | Link                                                                                   |
| -------------------------- | ------------------------------------------------------------- | ------ | -------------------------------------------------------------------------------------- |
| `main`                     | NET Core 10 solution templates, working content only          | 🟢     | [🔗](https://github.com/carloswm85/dotnet-template-kit/)                               |
| `dev-01`                   | Development branch, NET Core 10 solution templates            | 🟡     | [🔗](https://github.com/carloswm85/dotnet-template-kit/)                               |
| `dev-lightweight-netcore8` | NET Core 8, NLayer Template                                   | 🟢     | [🔗](https://github.com/carloswm85/dotnet-template-kit/tree/dev-netcore08-lightweight) |
| `dev-identity-netcore10`   | NET Core 8, NLayer Template, with Identity API implementation | 🟢     | [🔗](https://github.com/carloswm85/dotnet-template-kit/tree/dev-netcore10-identity)    |

`dev-*` is used for stable versions.

---

## 3. Additional Documentation

### Repository Documentation

- [README.md](./docs/README.md)

### Online

- Microsoft documentation:
  - <https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/>
  - <https://learn.microsoft.com/en-us/azure/architecture/>
- Architectures:
  - Modular Monolithic:
    - ?
  - Clean Architecture:
    - ?
  