# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c release -o out
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PlatformService.dll"]
#COPY ["PlatformService/PlatformService.csproj", "PlatformService/"]
#RUN dotnet restore "./PlatformService/PlatformService.csproj"
#COPY . .
#WORKDIR "/src/PlatformService"
#RUN dotnet build "./PlatformService.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
# This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./PlatformService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "PlatformService.dll"]