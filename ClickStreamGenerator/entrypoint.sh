#!/bin/bash
cd /app/publish & dotnet ClickStreamGenerator.dll $hostName $sasKeyName $sasKeyValue $eventHubName