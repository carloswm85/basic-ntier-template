- [Basic Solution Template](#basic-solution-template)
  - [How To Use](#how-to-use)
    - [Connection String](#connection-string)
    - [Entity Framework Migration Commands](#entity-framework-migration-commands)
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

# Basic Solution Template

## How To Use

1. Install from local folder.
2. At root level `basic-solution-template` run:

```powershell
# .\templates\MyCustomTemplate
dotnet new install .
```

Or:

```powershell
# .\templates\MyCustomTemplate
dotnet new uninstall .
```

3. Use the template from anywhere:

```powershell
dotnet new basic-sol-template -o "MyAmazingDotNetSuperSolutionExample"
```

### Connection String

```json
{
	"ConnectionStrings": {
		"DefaultConnection": "Server=.;Database=MyCustomTemplateData;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
	}
}
```

1. `Server=.;`

   - Specifies the SQL Server instance to connect to.
   - Selected server is: `Default Server`

2. `Database=MyCustomTemplateData;`

   - Name of the **database** you want to connect to.

3. `Trusted_Connection=True;`

   - Uses **Windows Authentication** (your Windows user credentials) instead of a SQL username/password.

4. `MultipleActiveResultSets=true;`

   - Enables **MARS**, allowing multiple queries to be active on the same connection at once (useful for EF and lazy loading).

5. `TrustServerCertificate=True;`

   - Skips certificate validation when using **encrypted connections** — useful for local or development setups where SSL certificates may not be trusted.

_Remember to change it for secure production-ready version (with SQL login and safer defaults)._

### Entity Framework Migration Commands

- Using PowerShell:

  - Inside the `MyCustomTemplate.Data` project folder.

    ```powershell
    > dotnet ef migrations add InitialMigration
    > dotnet ef database update
    ```

- Using Package Manager:

  - Set "Default Project" to `MyCustomTemplate.Data`

    ```console
    PM> Add-Migration InitialMigration
    PM> Update-Database
    ```

---

## Template Information

### AutoMapper Use

```
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
