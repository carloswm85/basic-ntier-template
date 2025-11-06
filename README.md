- [Basic Ntier Template](#basic-ntier-template)
	- [Version compatibility](#version-compatibility)
		- [.NET Core](#net-core)
		- [Angular](#angular)
	- [Installation Requirements](#installation-requirements)
	- [Architecture](#architecture)
		- [Notes](#notes)
	- [Template Installation](#template-installation)
		- [Nuget Installation](#nuget-installation)
		- [Local Repository Installation](#local-repository-installation)
		- [Installation Commands](#installation-commands)
	- [Entity Framework Core](#entity-framework-core)
		- [Code-First](#code-first)
		- [Database-First (Database Scaffolding, or Reverse Engineering)](#database-first-database-scaffolding-or-reverse-engineering)
			- [Additional Commands](#additional-commands)
		- [Example: `Blog` and `Post` models](#example-blog-and-post-models)
	- [Template Information](#template-information)
		- [Connection String](#connection-string)
		- [AutoMapper Use](#automapper-use)
	- [Troubleshooting](#troubleshooting)
		- [HTTPS Developer Ccertificate](#https-developer-ccertificate)
	- [Other Information](#other-information)
		- [Readings](#readings)
		- [Video Tutorials](#video-tutorials)
		- [Other Custom .NET Solution Templates](#other-custom-net-solution-templates)
		- [Recommended Tools](#recommended-tools)
		- [Clean Architecture Information](#clean-architecture-information)
			- [Readings (CA)](#readings-ca)
			- [Video Tutorials (CA)](#video-tutorials-ca)
	- [Videos](#videos)
		- [Project Videos](#project-videos)

---

**🛑 STILL UNDER DEVELOPMENT - USE WITH CAUTION 🛑**

# Basic Ntier Template

<https://github.com/carloswm85/basic-ntier-template>

## Version compatibility

### .NET Core

| Current | .NET Core | .NET Core release type | EF Core | Bootstrap |
| ------- | --------- | ---------------------- | ------- | --------- |
| ✅      | `8.0.100` | LTS                    | `8.0`   | `5.3.8`   |
|         | `9`       | STS                    | -       | -         |
|         | `10`      | pre-release            | -       | -         |

- [.NET and .NET Core Support Policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) ↗

### Angular

| Current | Angular Version      | Node.js Version                       | TypeScript Version | RxJS Version         |
| ------- | -------------------- | ------------------------------------- | ------------------ | -------------------- |
|         | `20.2.x` or `20.3.x` | `^20.19.0` or `^22.12.0` or `^24.0.0` | `>=5.9.0 <6.0.0`   | `^6.5.3` or `^7.4.0` |
|         | `20.0.x` or `20.1.x` | `^20.19.0` or `^22.12.0` or `^24.0.0` | `>=5.8.0 <5.9.0`   | `^6.5.3` or `^7.4.0` |
| ✅      | `19.2.x`             | `^18.19.1` or `^20.11.1` or `^22.0.0` | `>=5.5.0 <5.9.0`   | `^6.5.3` or `^7.4.0` |
|         | `19.1.x`             | `^18.19.1` or `^20.11.1` or `^22.0.0` | `>=5.5.0 <5.8.0`   | `^6.5.3` or `^7.4.0` |
|         | `19.0.x`             | `^18.19.1` or `^20.11.1` or `^22.0.0` | `>=5.5.0 <5.7.0`   | `^6.5.3` or `^7.4.0` |
|         | `18.1.x` or `18.2.x` | `^18.19.1` or `^20.11.1` or `^22.0.0` | `>=5.4.0 <5.6.0`   | `^6.5.3` or `^7.4.0` |
|         | `18.0.x`             | `^18.19.1` or `^20.11.1` or `^22.0.0` | `>=5.4.0 <5.5.0`   | `^6.5.3` or `^7.4.0` |

---

## Installation Requirements

- [Installation requirements](./docs/content/installation-requirements.md)

---

## Architecture

| Level | Layers        | Classification                     | Functionality                                          | Technology/Notes                                                   | Role                                                                             |
| ----- | ------------- | ---------------------------------- | ------------------------------------------------------ | ------------------------------------------------------------------ | -------------------------------------------------------------------------------- |
| 1     | `Data`        | -                                  | Database schema, migrations, models                    | Entity Framework Core, Identity API integration                    | Define database schema, manage migrations, and entity mapping.                   |
| 2     | `Repository`  | -                                  | Data access abstraction, caching, Unit of Work Pattern | Querying interfaces                                                | Abstract data access logic to decouple the service layer from EF Core specifics. |
| 3     | `Service`     | Business logic                     | Logic, validation                                      | Business rules, support async methods                              | Implement business logic, enforce rules and validation.                          |
| 4.a   | `API`         | Request handling, Dev presentation | RESTful API, documentation                             | Swagger/OpenAPI, versioning                                        | Expose business services as RESTful endpoints.                                   |
| 4.b   | `Web.MVC`     | User presentation                  | Server-side rendering, UI logic                        | MVC design pattern, Razor syntax                                   | Traditional server-rendered UI using MVC and Razor Pages.                        |
| 4.c   | `Web.Angular` | User presentation                  | Client-side SPA                                        | Angular SPA, with strong typing (TypeScript), Rest API integration | Modern client-side SPA experience.                                               |

![Basic Ntier Template Architecture Diagram](./docs/img/basic-ntier-template-architecture-diagram.png)

### Notes

- Data Layer:
  - Etity Framework Core, with code-first and database-first support.
- Repository:
  - Define interfaces for repositories (e.g., IUserRepository) which support CRUD and query operations asynchronously.
  - Caching: Caching strategies, or as a decorator pattern, to optimize repeated data retrieval.
  - Unit of Work Pattern: Aggregate repository transactions ensuring atomicity.

## Template Installation

### Nuget Installation

TODO

### Local Repository Installation

1. Clone or download repository.
1. Install from local folder.
1. At root level `basic-ntier-template` run:

   ```powershell
   # basic-ntier-template\.
   dotnet new install .
   ```

   Or:

   ```powershell
   # basic-ntier-template\.
   dotnet new uninstall .
   ```

1. Use the template from anywhere:

```powershell
dotnet new basic-ntier-template -o "BasicNtierTemplateExample"
```

### Installation Commands

```powershell
-o # Custom solution name
```

---

## Entity Framework Core

### Code-First

- Using PowerShell:

  - Inside the `BasicNtierTemplate.Data` project folder.

  ```powershell
  > dotnet ef migrations add InitialMigration
  > dotnet ef database update
  ```

- Using Package Manager Console:

  - Set "Default Project" to `BasicNtierTemplate.Data`

  ```console
  PM> Add-Migration InitialMigration
  PM> Update-Database
  ```

### Database-First (Database Scaffolding, or Reverse Engineering)

Your database should be named `MyDatabaseDb` for scaffolding to work out-of-the-box. Otherwise, you will need to modify the some parts of the code to make it work.

<https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/>

- Inside the `BasicNtierTemplate.Data` project folder, using Package Manager Console:

  - When using directly the connection string in the command line:

  ```console
  PM> Scaffold-DbContext "Server=.;Database=BasicNtierTemplateDb;user id=SomeUser;password=ThisIsSomePassword;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
  ```

  - When extracting the connection string from `appsettings.json` in the `BasicNtierTemplate.API` project:

  ```console
  PM> Scaffold-DbContext "Name=BasicNtierTemplateConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
  ```

The previous command will overrite the existing `BasicNtierTemplateContext.cs` file.

#### Additional Commands

```console
PM>  Scaffold-DbContext "Name=BasicNtierTemplateConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model -Project BasicNtierTemplate.Data -StartupProject BasicNtierTemplate.API -Force -UseDatabaseNames -NoPluralize
```

Or using .NET Core CLI:

```console
dotnet ef dbcontext scaffold "Name=BasicNtierTemplateConnection" Microsoft.EntityFrameworkCore.SqlServer --output-dir Model --project BasicNtierTemplate.Data --startup-project BasicNtierTemplate.API --force --use-database-names --no-pluralize
```

### Example: `Blog` and `Post` models

This template has a built in example case for use and testing of database, EF Core and API endpoints (with `Sample` service, repository, interfaces and API resource name).

For additional information on this example, see: <https://learn.microsoft.com/en-us/ef/core/modeling/relationships>

---

## Template Information

### Connection String

```json
{
	"ConnectionStrings": {
		"DefaultConnection": "Server=.;Database=BasicNtierTemplateData;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
	}
}
```

1. `Server=.;`

   - Specifies the SQL Server instance to connect to.
   - Selected server is: `Default Server`

2. `Database=BasicNtierTemplateData;`

   - Name of the **database** you want to connect to.

3. `Trusted_Connection=True;`

   - Uses **Windows Authentication** (your Windows user credentials) instead of a SQL username/password.

4. `MultipleActiveResultSets=true;`

   - Enables **MARS**, allowing multiple queries to be active on the same connection at once (useful for EF and lazy loading).

5. `TrustServerCertificate=True;`

   - Skips certificate validation when using **encrypted connections** — useful for local or development setups where SSL certificates may not be trusted.

_Remember to change it for secure production-ready version (with SQL login and safer defaults)._

### AutoMapper Use

```console
YourApp.Api/  (Presentation)
├── Program.cs  → builder.Services.AddAutoMapper()
└── [References Service Layer]

YourApp.Services/  (Business Logic)
├── Mappings/
│   └── UserProfile.cs  → Profile classes
├── UserService.cs  → Uses IMapper
└── [Needs: AutoMapper package]

YourApp.Data/  (Data Access)
└── [No AutoMapper needed here]
```

---

## Troubleshooting

### HTTPS Developer Ccertificate

```powershell
> dotnet dev-certs https --clean
> dotnet dev-certs https --trust
> dotnet dev-certs https --check
```

---

## Other Information

### Readings

Official documentation:

- [Tutorial: Create a project template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template)
- [Manage .NET project and item templates](https://learn.microsoft.com/en-us/dotnet/core/install/templates)
- [Default templates for `dotnet new`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates)
- [Custom templates for `dotnet new`](<[https://](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)>)
- Templating syntax:
  - <https://github.com/dotnet/templating/wiki/Conditional-processing-and-comment-syntax>

Other documentation:

- <https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx>

### Video Tutorials

- [How To Create Your Own Templates in NET](https://youtu.be/rdWZo5PD9Ek)

### Other Custom .NET Solution Templates

Good:

- <https://github.com/jasontaylordev/CleanArchitecture>
- <https://github.com/ardalis/CleanArchitecture>

Average:

- <https://github.com/dorlugasigal/MiniClean.Template>

### Recommended Tools

- <https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools>

---

### Clean Architecture Information

#### Readings (CA)

- [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html), by Uncle Bob

#### Video Tutorials (CA)

1. [Clean Architecture with ASP.NET Core 3.0 - Jason Taylor - NDC Sydney 2019](https://www.youtube.com/watch?v=5OtUm1BLmG0)
2. [💻 Clean Architecture (by Syntax Async)](https://www.youtube.com/playlist?list=PLlKSF1mm1dufAZmGexWGnORX-yKiGpFjM) playlist

---

## Videos

### Project Videos

- [Week 4](https://youtu.be/RSQXpOOT220)
- [Week 5](https://youtu.be/TAKPFr6R8Ac)
