#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/MPStore.Cliente.API/MPStore.Cliente.API.csproj", "src/services/MPStore.Cliente.API/"]
COPY ["src/building blocks/MPStore.MessageBus/MPStore.MessageBus.csproj", "src/building blocks/MPStore.MessageBus/"]
COPY ["src/building blocks/MPStore.Core/MPStore.Core.csproj", "src/building blocks/MPStore.Core/"]
COPY ["src/building blocks/MPStore.WebAPI.Core/MPStore.WebAPI.Core.csproj", "src/building blocks/MPStore.WebAPI.Core/"]
RUN dotnet restore "src/services/MPStore.Cliente.API/MPStore.Cliente.API.csproj"
COPY . .
WORKDIR "/src/src/services/MPStore.Cliente.API"
RUN dotnet build "MPStore.Cliente.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MPStore.Cliente.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MPStore.Cliente.API.dll"]