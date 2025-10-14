- [Basic Ntier Template](#basic-ntier-template)
  - [How To Use](#how-to-use)
    - [Nuget Installation](#nuget-installation)
    - [Local Installation](#local-installation)
    - [Installation Commands](#installation-commands)
    - [Connection String](#connection-string)
    - [Entity Framework Core: Code-First](#entity-framework-core-code-first)
    - [Entity Framework Core: Database-First (Database Scaffolding, or Reverse Engineering)](#entity-framework-core-database-first-database-scaffolding-or-reverse-engineering)
  - [Template Information](#template-information)
    - [AutoMapper Use](#automapper-use)
  - [Troubleshooting](#troubleshooting)
    - [HTTPS Developer Ccertificate](#https-developer-ccertificate)
  - [Other Information](#other-information)
    - [Readings](#readings)
    - [Video Tutorials](#video-tutorials)
    - [Other .NET Solution Templates](#other-net-solution-templates)
    - [Clean Architecture Information](#clean-architecture-information)
      - [Readings (CA)](#readings-ca)
      - [Video Tutorials (CA)](#video-tutorials-ca)

---

# Basic Ntier Template

<https://github.com/carloswm85/basic-ntier-template>

## How To Use

### Nuget Installation

TODO

### Local Installation

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

### Entity Framework Core: Code-First

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

### Entity Framework Core: Database-First (Database Scaffolding, or Reverse Engineering)

- Inside the `BasicNtierTemplate.Data` project folder, using Package Manager Console:

  - When using directly the connection string from the command line:

    ```console
    PM> Scaffold-DbContext "Server=.;Database=BasicNtierTemplateDb;user id=SomeUser;password=ThisIsSomePassword;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
    ```

- When extracting the connection string from `appsettings.json` in the `BasicNtierTemplate.API` project:

```console
Scaffold-DbContext "Name=BasicNtierTemplateConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model -Project BasicNtierTemplate.Data -StartupProject BasicNtierTemplate.API -Force
```

The previous command will overrite the existing `BasicNtierTemplateContext.cs` file.

---

## Template Information

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

Official Documentation:

- [Tutorial: Create a project template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template)
- [Manage .NET project and item templates](https://learn.microsoft.com/en-us/dotnet/core/install/templates)
- [Default templates for `dotnet new`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates)
- [Custom templates for `dotnet new`](<[https://](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)>)
- Templating syntax:
  - <https://github.com/dotnet/templating/wiki/Conditional-processing-and-comment-syntax>

### Video Tutorials

1. [How To Create Your Own Templates in NET](https://youtu.be/rdWZo5PD9Ek)

### Other .NET Solution Templates

Good:

- <https://github.com/jasontaylordev/CleanArchitecture>
- <https://github.com/ardalis/CleanArchitecture>

Average:

- <https://github.com/dorlugasigal/MiniClean.Template>

---

### Clean Architecture Information

#### Readings (CA)

- [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html), by Uncle Bob

#### Video Tutorials (CA)

1. [Clean Architecture with ASP.NET Core 3.0 - Jason Taylor - NDC Sydney 2019](https://www.youtube.com/watch?v=5OtUm1BLmG0)
2. [💻 Clean Architecture (by Syntax Async)](https://www.youtube.com/playlist?list=PLlKSF1mm1dufAZmGexWGnORX-yKiGpFjM) playlist
