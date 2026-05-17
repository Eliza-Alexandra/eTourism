# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY eTourismWebApp/eTourismWebApp.csproj ./eTourismWebApp/
COPY DataLayer/DataLayer.csproj ./DataLayer/

# Restore dependencies
RUN dotnet restore eTourismWebApp/eTourismWebApp.csproj

# Copy everything
COPY . .

# Publish
RUN dotnet publish eTourismWebApp/eTourismWebApp.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "eTourismWebApp.dll"]
