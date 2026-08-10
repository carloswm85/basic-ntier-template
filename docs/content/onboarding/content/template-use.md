- [Template Installation And Use](#template-installation-and-use)
    - [From Nuget](#from-nuget)
    - [From Local Repository](#from-local-repository)
    - [Using The Template](#using-the-template)

---

# Template Installation And Use

## From Nuget

// TODO

## From Local Repository

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

## Using The Template

```powershell
dotnet new basic-ntier-template -o "../../MyFolder/BasicNtierTemplateExample2" # Custom solution name (and path, if included in the string)
```

```powershell
dotnet new basic-ntier-template -o  "BasicNtierTemplateExample3" --force # Force file generation, and override existing files if any
```
