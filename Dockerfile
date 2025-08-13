FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar la solución y el csproj raíz
COPY ["ecommerceAPI.sln", "."]
COPY ["ecommerceAPI.csproj", "."]

# Restaurar dependencias
RUN dotnet restore "ecommerceAPI.csproj"

# Copiar todo el código
COPY . .

# Compilar en Release
RUN dotnet build "ecommerceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ecommerceAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Crear carpeta de logs
RUN mkdir -p /app/logs

# Ejecutar
ENTRYPOINT ["dotnet", "ecommerceAPI.dll"] 