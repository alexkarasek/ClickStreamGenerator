#!/bin/bash
cd /app/publish & 
sh -c "dotnet ClickStreamGenerator.dll \"$hostName\" \"$sasKeyName\" \"$sasKeyValue\" \"$eventHubName\""
