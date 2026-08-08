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
ENV ConnectionStrings__DefaultConnection="Server=sqlserver;Database=NLayerTemplateDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True"

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["solution/NLayerTemplate.Web.MVC/NLayerTemplate.Web.MVC.csproj", "solution/NLayerTemplate.Web.MVC/"]
COPY ["solution/NLayerTemplate.Data/NLayerTemplate.Data.csproj", "solution/NLayerTemplate.Data/"]
COPY ["solution/NLayerTemplate.Service/NLayerTemplate.Service.csproj", "solution/NLayerTemplate.Service/"]
COPY ["solution/NLayerTemplate.Repository/NLayerTemplate.Repository.csproj", "solution/NLayerTemplate.Repository/"]
RUN dotnet restore "./solution/NLayerTemplate.Web.MVC/NLayerTemplate.Web.MVC.csproj"
COPY . .
WORKDIR "/src/solution/NLayerTemplate.Web.MVC"
RUN dotnet build "./NLayerTemplate.Web.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NLayerTemplate.Web.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NLayerTemplate.Web.MVC.dll"]
