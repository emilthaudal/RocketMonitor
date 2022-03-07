FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://*:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RocketMonitor/RocketMonitor.csproj", "RocketMonitor/"]
COPY ["RocketMonitor.Domain/RocketMonitor.Domain.csproj", "RocketMonitor.Domain/"]
COPY ["RocketMonitor.Service/RocketMonitor.Service.csproj", "RocketMonitor.Service/"]
COPY ["RocketMonitor.Infrastructure/RocketMonitor.Infrastructure.csproj", "RocketMonitor.Infrastructure/"]
COPY ["RocketMonitor.MemoryEventStore/RocketMonitor.MemoryEventStore.csproj", "RocketMonitor.MemoryEventStore/"]
RUN dotnet restore "RocketMonitor/RocketMonitor.csproj"
COPY . .
WORKDIR "/src/RocketMonitor"
RUN dotnet build "RocketMonitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RocketMonitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RocketMonitor.dll"]
