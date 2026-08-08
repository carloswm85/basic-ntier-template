- [DotNet Template Kit](#dotnet-template-kit)
  - [(1) Available templates](#1-available-templates)
  - [(2) Requirements](#2-requirements)
  - [(3) Installation](#3-installation)
  - [(4) Quick start](#4-quick-start)
  - [(5) Template options](#5-template-options)
  - [(6) Documentation](#6-documentation)
  - [(7) Project status](#7-project-status)
  - [(8) Contributing](#8-contributing)
  - [(9) License](#9-license)

---

<https://github.com/carloswm85/dotnet-template-kit>

![DotNet Template Kit banner](./docs/img/banner.png)
![Under Construction](./docs/img/under-construction.jpg)

---

# DotNet Template Kit

DotNet Template Kit is a collection of opinionated .NET 10 solution templates
for building web applications with architectures of increasing complexity.

Choose a lightweight monolith for smaller applications, an N-layer solution for
clear separation of concerns, or a Clean Architecture template for domain-rich
systems with strict dependency boundaries.

---

## (1) Available templates

- `✅ Ready - 🚧 In progress - 📋 Planned - ⛔ Blocked`
- Increasing complexity, top to bottom.

| Code    | Architecture                     | Status | Recommended for                                                  | Template command                             | Documentation                                                                 |
| ------- | -------------------------------- | ------ | ---------------------------------------------------------------- | -------------------------------------------- | ----------------------------------------------------------------------------- |
| `SMA`   | Simple Monolith                  | ✅     | CRUD applications, internal tools, and MVPs                      | `dotnet new dtk-simple-monolith`             | [Simple Monolith documentation](./templates/SimpleMonolithTemplate/README.md) |
| `CNLA`  | N-Layer                          | ⛔     | Enterprise applications, large teams, and long-term maintenance  | `dotnet new dtk-complex-nlayer`              | [Complex N-Layer documentation](./templates/ComplexNLayerTemplate/README.md)  |
| `VSA`   | Vertical Slice                   | ⛔     | Independent API features, microservices, and modular monoliths   | Not available yet                            | [Vertical Slice documentation](./templates/VerticalSliceTemplate/README.md)   |
| `CNA-I` | Clean Architecture with Identity | ⛔     | Clean Architecture applications requiring ASP\.NET Core Identity | `dotnet new dtk-clean-architecture-identity` | [Identity template documentation](./templates/IdentityTemplate/README.md)     |

## (2) Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- A development environment such as Visual Studio, Visual Studio Code, or JetBrains Rider
- Docker Desktop for templates that provide container orchestration
- Node.js and Angular tooling when using a template that includes an Angular client

Run the following command to verify the installed .NET SDKs:

```powershell
dotnet --list-sdks
```

---

## (3) Installation

Clone the repository and install its templates from the repository root:

```powershell
git clone https://github.com/carloswm85/dotnet-template-kit.git
Set-Location ./dotnet-template-kit
dotnet new install .
```

List the installed DTK templates:

```powershell
dotnet new list dtk
```

To identify the registration name before uninstalling the templates, run:

```powershell
dotnet new uninstall
```

Then pass the listed package or directory identifier to `dotnet new uninstall`.

---

## (4) Quick start

Create a Basic N-Layer solution named `Contoso.BackOffice`:

```powershell
dotnet new dtk-basic-nlayer --name Contoso.BackOffice
Set-Location ./Contoso.BackOffice
dotnet restore
```

Use another command from the [available templates](#1-available-templates)
table to select a different architecture.

---

## (5) Template options

The available templates target .NET 10 and support the following shared options:

| Option                   | Type     | Default           | Description                                                  |
| ------------------------ | -------- | ----------------- | ------------------------------------------------------------ |
| `--name`                 | `string` | Template-specific | Sets the generated solution and project name                 |
| `--TargetFramework`      | `choice` | `net10.0`         | Selects the target framework                                 |
| `--IncludeDocumentation` | `bool`   | `false`           | Includes explanatory documentation in the generated solution |

Inspect every option supported by a template before creating a solution:

```powershell
dotnet new dtk-basic-nlayer --help
```

Example with documentation included:

```powershell
dotnet new dtk-basic-nlayer `
    --name Contoso.BackOffice `
    --IncludeDocumentation true
```

---

## (6) Documentation

- [Project documentation](./docs/README.md)
- [Microsoft .NET application architecture](https://learn.microsoft.com/dotnet/architecture/)
- [Azure Architecture Center](https://learn.microsoft.com/azure/architecture/)
- [.NET architecture guides](https://github.com/dotnet-architecture/eBooks)
- Clean Architecture examples:
  - [Ardalis CleanArchitecture](https://github.com/ardalis/CleanArchitecture)
  - [Jason Taylor CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture)

---

## (7) Project status

The `main` branch contains the latest supported templates and documentation.
Templates still under development are identified in the
[available templates](#1-available-templates) table.

---

## (8) Contributing

Issues and pull requests are welcome. Before proposing a change:

- Verify that it targets a supported template.
- Keep architecture-specific changes inside the corresponding template directory.
- Update the relevant template documentation.
- Confirm that the template installs and creates a new solution successfully.

---

## (9) License

DotNet Template Kit is available under the terms of the
[MIT License](./LICENSE.txt).
