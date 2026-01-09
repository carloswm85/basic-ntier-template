- [Troubleshooting](#troubleshooting)
  - [HTTPS Developer ASP.NET Certificate](#https-developer-aspnet-certificate)
  - [Angular Project](#angular-project)
    - [Running SPA from NET Core](#running-spa-from-net-core)

---

# Troubleshooting

## HTTPS Developer ASP.NET Certificate

```powershell
> dotnet dev-certs https --clean
> dotnet dev-certs https --trust
> dotnet dev-certs https --check
```

## Angular Project

### Running SPA from NET Core

1. At `BasicNtierTemplate.API\Properties\launchSettings.json`, uncomment lines:

```json
"ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.SpaProxy"
```

2. At `BasicNtierTemplate.API\BasicNtierTemplate.API.csproj` uncomment:

```xml
<!-- Angular Project
-->
<SpaProxyLaunchCommand>npm start</SpaProxyLaunchCommand>
<SpaRoot>..\BasicNtierTemplate.Web.Angular</SpaRoot>
<SpaProxyServerUrl>https://localhost:5021</SpaProxyServerUrl>
```

3. When debugging `BasicNtierTemplate.API`, the Angular SPA will be launch (`BasicNtierTemplate.Web.Angular`).

Working demo page, fetching data from API layer to Angular layer:

![Working endpoing](./img/angular-working-endpoint.png)
