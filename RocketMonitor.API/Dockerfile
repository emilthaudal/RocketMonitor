FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://*:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RocketMonitor.API/RocketMonitor.API.csproj", "RocketMonitor.API/"]
COPY ["RocketMonitor.Domain/RocketMonitor.Domain.csproj", "RocketMonitor.Domain/"]
COPY ["RocketMonitor.Service/RocketMonitor.Service.csproj", "RocketMonitor.Service/"]
COPY ["RocketMonitor.Infrastructure/RocketMonitor.Infrastructure.csproj", "RocketMonitor.Infrastructure/"]
COPY ["RocketMonitor.MemoryEventStore/RocketMonitor.MemoryEventStore.csproj", "RocketMonitor.MemoryEventStore/"]
RUN dotnet restore "RocketMonitor.API/RocketMonitor.API.csproj"
COPY . .
WORKDIR "/src/RocketMonitor.API"
RUN dotnet build "RocketMonitor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RocketMonitor.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RocketMonitor.API.dll"]
