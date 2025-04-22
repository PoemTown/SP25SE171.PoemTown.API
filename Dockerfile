#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

COPY ./PoemTown.API/Views /app/Views
COPY ./PoemTown.Service/PlagiarismDetector /app/PlagiarismDetector

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PoemTown.API/PoemTown.API.csproj", "PoemTown.API/"]
COPY ["PoemTown.Service/PoemTown.Service.csproj", "PoemTown.Service/"]
COPY ["PoemTown.Repository/PoemTown.Repository.csproj", "PoemTown.Repository/"]
RUN dotnet restore "./PoemTown.API/PoemTown.API.csproj"
COPY . .
WORKDIR "/src/PoemTown.API"
RUN dotnet build "./PoemTown.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PoemTown.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PoemTown.API.dll"]
