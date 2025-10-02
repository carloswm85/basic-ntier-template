- [Basic Solution Template](#basic-solution-template)
  - [How To Use](#how-to-use)
  - [Template Information](#template-information)
    - [Readings](#readings)
    - [Video Tutorials](#video-tutorials)
    - [Other .NET Solution Templates](#other-net-solution-templates)
  - [Clean Architecture](#clean-architecture)
    - [Readings (CA)](#readings-ca)
    - [Video Tutorials (CA)](#video-tutorials-ca)

---

# Basic Solution Template

## How To Use

1. Install from local folder.
2. At root level run:

```powershell
# .\basic-solution-template\My.Custom.Template
dotnet new install .
```

Or:

```powershell
# .\basic-solution-template\My.Custom.Template
dotnet new uninstall .
```

3. Use the template from anywhere:

```powershell
dotnet new basic-sol-template -o "MyAmazingDragonOfFire"
```

---

## Template Information

### Readings

Official Documentation:

- [Tutorial: Create a project template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template)
- [Manage .NET project and item templates](https://learn.microsoft.com/en-us/dotnet/core/install/templates)
- [Default templates for `dotnet new`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates)

### Video Tutorials

1. [How To Create Your Own Templates in NET](https://youtu.be/rdWZo5PD9Ek)

### Other .NET Solution Templates

Good:

- <https://github.com/jasontaylordev/CleanArchitecture>
- <https://github.com/ardalis/CleanArchitecture>

Average:

- <https://github.com/dorlugasigal/MiniClean.Template>

---

## Clean Architecture

### Readings (CA)

- [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html), by Uncle Bob

### Video Tutorials (CA)

1. [Clean Architecture with ASP.NET Core 3.0 - Jason Taylor - NDC Sydney 2019](https://www.youtube.com/watch?v=5OtUm1BLmG0)
2. [💻 Clean Architecture (by Syntax Async)](https://www.youtube.com/playlist?list=PLlKSF1mm1dufAZmGexWGnORX-yKiGpFjM) playlist
