# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Environment variables for development.
# These can be overridden when running the container in production or when running from VS in regular mode (Default when not using the Debug configuration)
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ConnectionStrings__DefaultConnection="Server=sqlserver;Database=ComplexNLayerTemplateDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True"

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["solution/ComplexNLayerTemplate.Web.MVC/ComplexNLayerTemplate.Web.MVC.csproj", "solution/ComplexNLayerTemplate.Web.MVC/"]
COPY ["solution/ComplexNLayerTemplate.Data/ComplexNLayerTemplate.Data.csproj", "solution/ComplexNLayerTemplate.Data/"]
COPY ["solution/ComplexNLayerTemplate.Service/ComplexNLayerTemplate.Service.csproj", "solution/ComplexNLayerTemplate.Service/"]
COPY ["solution/ComplexNLayerTemplate.Repository/ComplexNLayerTemplate.Repository.csproj", "solution/ComplexNLayerTemplate.Repository/"]
RUN dotnet restore "./solution/ComplexNLayerTemplate.Web.MVC/ComplexNLayerTemplate.Web.MVC.csproj"
COPY . .
WORKDIR "/src/solution/ComplexNLayerTemplate.Web.MVC"
RUN dotnet build "./ComplexNLayerTemplate.Web.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ComplexNLayerTemplate.Web.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ComplexNLayerTemplate.Web.MVC.dll"]