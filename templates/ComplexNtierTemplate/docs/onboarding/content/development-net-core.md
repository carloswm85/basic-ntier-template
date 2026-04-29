- [NET Core Dependencies](#net-core-dependencies)
  - [Use Entity Framework Core](#use-entity-framework-core)
    - [Code-First](#code-first)
    - [Database-First (Database Scaffolding, or Reverse Engineering)](#database-first-database-scaffolding-or-reverse-engineering)
      - [Additional Commands](#additional-commands)
  - [Configuration Information](#configuration-information)
    - [Connection String](#connection-string)
    - [AutoMapper Use](#automapper-use)

---

# NET Core Dependencies

## Use Entity Framework Core

Be sure you have the right tools: [Installation requirements](./docs/content/installation-requirements.md)

### Code-First

- Using PowerShell:
  - Inside the `ComplexNtierTemplate.Data` project folder run:

  ```powershell
  > dotnet ef migrations add InitialMigration
  > dotnet ef database update
  ```

- Using Package Manager Console:
  - Set "Default Project" to `ComplexNtierTemplate.Data`, and run:

  ```console
  PM> Add-Migration InitialMigration
  PM> Update-Database
  ```

### Database-First (Database Scaffolding, or Reverse Engineering)

Your database should be named `MyDatabaseDb` for scaffolding to work out-of-the-box. Otherwise, you will need to modify the some parts of the code to make it work.

<https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/>

- Inside the `ComplexNtierTemplate.Data` project folder, using Package Manager Console:
  - When using directly the connection string in the command line:

  ```console
  PM> Scaffold-DbContext "Server=.;Database=ComplexNtierTemplateDb;user id=SomeUser;password=ThisIsSomePassword;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
  ```

  - When extracting the connection string from `appsettings.json` in the `ComplexNtierTemplate.Web.API` project:

  ```console
  PM> Scaffold-DbContext "Name=ComplexNtierTemplateConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model
  ```

The previous command will overrite the existing `ComplexNtierTemplateContext.cs` file.

#### Additional Commands

```console
PM>  Scaffold-DbContext "Name=ComplexNtierTemplateConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model -Project ComplexNtierTemplate.Data -StartupProject ComplexNtierTemplate.Web.API -Force -UseDatabaseNames -NoPluralize
```

Or using .NET Core CLI:

```console
dotnet ef dbcontext scaffold "Name=ComplexNtierTemplateConnection" Microsoft.EntityFrameworkCore.SqlServer --output-dir Model --project ComplexNtierTemplate.Data --startup-project ComplexNtierTemplate.Web.API --force --use-database-names --no-pluralize
```

---

## Configuration Information

### Connection String

```json
{
	"ConnectionStrings": {
		"DefaultConnection": "Server=.;Database=ComplexNtierTemplateDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
	}
}
```

1. `Server=.;`
   - Specifies the SQL Server instance to connect to.
   - Selected server is: `Default Server`

2. `Database=ComplexNtierTemplateData;`
   - Name of the **database** you want to connect to.

3. `Trusted_Connection=True;`
   - Uses **Windows Authentication** (your Windows user credentials) instead of a SQL username/password.

4. `MultipleActiveResultSets=true;`
   - Enables **MARS**, allowing multiple queries to be active on the same connection at once (useful for EF and lazy loading).

5. `TrustServerCertificate=True;`
   - Skips certificate validation when using **encrypted connections** — useful for local or development setups where SSL certificates may not be trusted.

_Remember to change it for secure production-ready version (with SQL login and safer defaults)._

---

### AutoMapper Use

```terminal
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
