# ClickStreamGenerator
Generate stream of data and publish to Kafka topic

This project was created to dynamically produce a stream of data using random values, and write that stream to a Kafka topic.  The stream produced includes:

sessionid => integer defining session  
productid => random productid generated as int between bounds described below  
timestamp => timestamp of simulated activity  
category => category id assigned based on product heirarchy

This project can be used to build a .Net console application that can be run from command line or as a Docker container.  

To run from [Docker Hub](https://hub.docker.com/repository/docker/alexk002/wwiclickstreamgenerator), use:

`docker run --name stream -it -e hostName="[Host Name]" -e sasKeyName="[SAS Key]" -e sasKeyValue="[SAS KEY VALUE]" -e eventHubName="[Event Hub Name]" stream`

**Note:** 
1. this method assumes that an Azure Event Hub has already been created.
1. this method also assumes that default values can be used for properties listed above

In order to run this project from command line:
1. pass in arguments for optional paramters [Host Name], [SAS Key Name], [SAS Key Value], and [Event Hub Name] in order OR
1. update appsettings.json file to assign values for properties below, and execute without passing in any arguments: 

brokerList => Azure Event Hub hostname 
topicName => name of Event Hub (topic)
sessionctr = 0;  //hard coded seed for session iterator  
maxclicks = max number of "clicks" per session  
maxProductId = upper bound for random productid to be generated  
minProductId = lower bound for random productid to be generated  
nosessions = number of sessions to generate  
nocats = number of categories that product heirarchy should span  

**Note:**
This project was developed to provide source data for streaming data exercise found [here](https://github.com/DataSciNAll/mdwguide).
