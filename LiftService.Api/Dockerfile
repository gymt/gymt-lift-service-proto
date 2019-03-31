# References:
# https://docs.docker.com/engine/examples/dotnetcore
# https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/docker-application-development-process/docker-app-development-workflow
# 

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

# Copy csproj and restore as distinct layers for a Multi-stage build
# Reference for the benefits of this:
# https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/docker-application-development-process/docker-app-development-workflow#multi-stage-builds-in-dockerfile
COPY ["LiftService.Api/LiftService.Api.csproj", "LiftService.Api/"]
COPY ["LiftService.Database/LiftService.Database.csproj", "LiftService.Database/"]
COPY ["LiftService.Domain/LiftService.Domain.csproj", "LiftService.Domain/"]
COPY ["LiftService.Controller/LiftService.Controller.csproj", "LiftService.Controller/"]
RUN dotnet restore "LiftService.Api/LiftService.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/LiftService.Api"
RUN dotnet build "LiftService.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "LiftService.Api.csproj" -c Release -o /app

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LiftService.Api.dll"]