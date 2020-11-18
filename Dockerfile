  
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app

COPY ./*.sln ./
COPY ./ClickStreamGenerator/ClickStreamGenerator.csproj ./ClickStreamGenerator/ClickStreamGenerator.csproj
RUN dotnet restore

COPY ./ClickStreamGenerator ./ClickStreamGenerator

RUN dotnet build 

FROM build AS publish
RUN dotnet publish "./ClickStreamGenerator/ClickStreamGenerator.csproj" -c Release -o /app/publish --no-restore
COPY ./ClickStreamGenerator/appsettings.json /app/publish/appsettings.json
COPY ./ClickStreamGenerator/cacert.pem /app/publish/cacert.pem
COPY ./ClickStreamGenerator/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# RUN ls /app/publish

RUN cp ./ClickStreamGenerator/appsettings.json ./publish/appsettings.json
RUN cp ./ClickStreamGenerator/cacert.pem ./publish/cacert.pem
# FROM base as final 

WORKDIR /app/publish

# ENV ConnectionString="Endpoint=sb://[HostName].servicebus.windows.net/;SharedAccessKeyName=[SAS Key Name];SharedAccessKey=[SAS Key];EntityPath=[Event Hub Name]"

# WORKDIR /src/ClickStreamGenerator
# COPY --from=publish /app/publish .
# # ENTRYPOINT [ "dotnet", "ClickStreamGenerator.dll" ]
# ENTRYPOINT ["./ClickStreamGenerator/dotnet run ClickStreamGenerator.dll", $ConnectionString]

# ENTRYPOINT ["/app/entrypoint.sh"]
CMD /app/entrypoint.sh