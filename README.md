# ClickStreamGenerator
Generate stream of data and publish to Kafka topic

This project was created to dynamically produce a stream of data using random values, and write that stream to a Kafka topic.  The stream produced includes:

sessionid => integer defining session  
productid => random productid generated as int between bounds described below  
timestamp => timestamp of simulated activity  
category => category id assigned based on product heiarchy

And parameter values (stored as variables inside of the code) should be updated to assign values for: 

brokerList => Kafka brokers 
topicName => name of Kafka topic  
sessionctr = 0;  //hard coded seed for session iterator  
maxclicks = max number of "clicks" per session  
maxProductId = upper bound for random productid to be generated  
minProductId = lower bound for random productid to be generated  
nosessions = number of sessions to generate  
nocats = number of categories that product heirarchy should span  

**Note:**
This project was developed to provide source data for streaming data exercise found [here](https://github.com/DataSciNAll/mdwguide).
