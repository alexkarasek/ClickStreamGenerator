FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app

COPY ./*.sln ./
COPY ./ClickStreamGenerator/ClickStreamGenerator.csproj ./ClickStreamGenerator/ClickStreamGenerator.csproj
RUN dotnet restore

COPY ./ClickStreamGenerator ./ClickStreamGenerator

RUN dotnet build -c Release --no-restore

FROM build AS publish
RUN dotnet publish "./ClickStreamGenerator/ClickStreamGenerator.csproj" -c Release -o /app/publish --no-restore
COPY ./ClickStreamGenerator/appsettings.json /app/publish/appsettings.json
COPY ./ClickStreamGenerator/cacert.pem /app/publish/cacert.pem
RUN ls /app/publish

FROM base as final 

WORKDIR /src/ClickStreamGenerator
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "ClickStreamGenerator.dll" ]