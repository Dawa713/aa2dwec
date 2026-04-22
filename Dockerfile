# Multi-stage build para optimizar imagen
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["api clase.csproj", "./"]

# Restaurar dependencias
RUN dotnet restore "api clase.csproj"

# Copiar código fuente
COPY . .

# Compilar
RUN dotnet build "api clase.csproj" -c Release -o /app/build

# Publicar
RUN dotnet publish "api clase.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime (imagen más pequeña)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Copiar binarios compilados
COPY --from=build /app/publish .

# Exponer puerto
EXPOSE 5000
EXPOSE 5001

# Variable de entorno
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:5000/swagger/index.html || exit 1

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "api clase.dll"]
