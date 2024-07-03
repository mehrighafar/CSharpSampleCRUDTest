#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /csharp.sample.crudtest.app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/csharp.sample.crudtest
COPY ["CSharpSampleCRUDTest.API/CSharpSampleCRUDTest.API.csproj", "CSharpSampleCRUDTest.API/"]
COPY ["CSharpSampleCRUDTest.DataAccess/CSharpSampleCRUDTest.DataAccess.csproj", "CSharpSampleCRUDTest.DataAccess/"]
COPY ["CSharpSampleCRUDTest.Domain/CSharpSampleCRUDTest.Domain.csproj", "CSharpSampleCRUDTest.Domain/"]
COPY ["CSharpSampleCRUDTest.Logic/CSharpSampleCRUDTest.Logic.csproj", "CSharpSampleCRUDTest.Logic/"]

COPY . .

WORKDIR /src/csharp.sample.crudtest/CSharpSampleCRUDTest.API
RUN dotnet clean
RUN dotnet restore

WORKDIR /src/csharp.sample.crudtest/CSharpSampleCRUDTest.API
RUN dotnet build "CSharpSampleCRUDTest.API.csproj" -c $BUILD_CONFIGURATION -o /csharp.sample.crudtest.app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CSharpSampleCRUDTest.API.csproj" -c $BUILD_CONFIGURATION -o /csharp.sample.crudtest.app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /csharp.sample.crudtest.app
COPY --from=publish /csharp.sample.crudtest.app/publish .
ENTRYPOINT ["dotnet", "CSharpSampleCRUDTest.API.dll"]