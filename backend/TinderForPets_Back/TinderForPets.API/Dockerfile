#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5294
EXPOSE 5295

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TinderForPets.API/TinderForPets.API.csproj", "TinderForPets.API/"]
COPY ["TinderForPets.Application/TinderForPets.Application.csproj", "TinderForPets.Application/"]
COPY ["TinderForPets.Data/TinderForPets.Data.csproj", "TinderForPets.Data/"]
COPY ["TinderForPets.Core/TinderForPets.Core.csproj", "TinderForPets.Core/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["TinderForPets.Infrastructure/TinderForPets.Infrastructure.csproj", "TinderForPets.Infrastructure/"]
RUN dotnet restore "./TinderForPets.API/TinderForPets.API.csproj"
COPY . .
WORKDIR "/src/TinderForPets.API"
RUN dotnet build "./TinderForPets.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TinderForPets.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TinderForPets.API.dll"]