  
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


# docker run --name stream -it -e hostName="[Host Name]" -e sasKeyName="[SAS Key]" -e sasKeyValue="[SAS KEY VALUE]" -e eventHubName="[Event Hub Name]" stream 
# docker build . -t alexk002/wwiclickstreamgenerator:1
# docker push alexk002/wwiclickstreamgenerator:1
# az container create -g dockercontainerRG --name [container name] --image alexk002/wwiclickstreamgenerator:1 --environment-variables 'hostName'='[EH Host Name]' 'sasKeyName'='RootManageSharedAccessKey' 'sasKeyValue'='[SAS Key]' 'eventHubName'='[Event Hub Name]' --cpu 1 --memory 3.5 