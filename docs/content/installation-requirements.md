# Installation Requirements

## NET Core Development

- <https://dotnet.microsoft.com/en-us/download/dotnet>

Specific version installation:

- Windows: `winget install Microsoft.DotNet.SDK.8 --version 8.0.100`

## Angular Development

### Install Node

- <https://nodejs.org/en/download>
- Or better:
  - In Mac/Linux: <https://github.com/nvm-sh/nvm>
  - In Windows: <https://github.com/coreybutler/nvm-windows/>
    - Usage:

       ```console
       > nvm list
       > nvm install lts
       > nvm install 19
       > nvm use 19
       ```

You can install multiple Node versions.

### Install Angular

Commands:

```console
> npm install -g @angular/cli@19.2.0
> ng version
```
