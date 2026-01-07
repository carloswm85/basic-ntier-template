- [Identity Scaffolding](#identity-scaffolding)
  - [Required Commands](#required-commands)
    - [Required NuGet Packages for Identity Scaffolding](#required-nuget-packages-for-identity-scaffolding)
    - [Global Tool Setup and Verification](#global-tool-setup-and-verification)
    - [Identity Code Generator Usage](#identity-code-generator-usage)
    - [Entity Framework Core Migrations](#entity-framework-core-migrations)
  - [Official Reasoning For Scaffolding Use (important)](#official-reasoning-for-scaffolding-use-important)

---

# Identity Scaffolding

- Documentation:
  - <https://learn.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-8.0>
- Videos:
  - <https://youtu.be/PmqPEB6RRBI>

## Required Commands

- In the right project folder run the following commands.

### Required NuGet Packages for Identity Scaffolding

```powershell
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Global Tool Setup and Verification

```powershell
dotnet tool list -g
dotnet tool install -g dotnet-aspnet-codegenerator --version 8.0.22
dotnet tool search dotnet-aspnet-codegenerator
dotnet tool search dotnet-aspnet-codegenerator --detail
```

### Identity Code Generator Usage

```powershell
dotnet aspnet-codegenerator identity -h
dotnet aspnet-codegenerator identity # full generation
```

```powershell
# Default UI and minimun number of files
dotnet aspnet-codegenerator identity --useDefaultUI
# Target DB context and generate selected files
dotnet aspnet-codegenerator identity -dc MyApplication.Data.ApplicationDbContext --files "Account.Register;Account.Login"
```

### Entity Framework Core Migrations

Using NET CLI tools:

```powershell
dotnet ef migrations add {MIGRATION NAME}
dotnet ef migrations add CreateIdentitySchema
dotnet ef database update
dotnet ef migrations list
```

```powershell
dotnet ef migrations remove
dotnet ef database drop
```

```powershell
# From a class library project
dotnet ef migrations add {MIGRATION_NAME} --startup-project {PATH_TO_APP_PROJECT}
```

Using [Package Manager Console](https://learn.microsoft.com/en-us/ef/core/cli/powershell) (PMC):

```terminal
Install-Package Microsoft.EntityFrameworkCore.Tools
Get-Migration
Add-Migration {MIGRATION NAME}
Update-Database
```

```terminal
Remove-Migration
Drop-Database -WhatIf
```

---

## Official Reasoning For Scaffolding Use (important)

Identity scaffolding does not support MVC scaffolding.

- Use Razor Pages (recommended). The tool uses a RCL (Razor Class Library)
  - <https://learn.microsoft.com/en-us/aspnet/core/razor-pages/ui-class?view=aspnetcore-8.0>
- Official
- Maintained
- Future-proof
- Works perfectly in MVC apps
  Microsoft intentionally moved Identity UI to Razor Pages because:
- Better encapsulation
- Cleaner routing
- Easier security hardening
- Less controller boilerplate
