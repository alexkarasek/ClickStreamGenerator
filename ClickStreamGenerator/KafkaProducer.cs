using Confluent.Kafka;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ClickStreamGenerator
{
    public class KafkaProducer
    {
        public  KafkaProducer(string connectionString, string brokerList, string topicName, string msg, string caCertLocation)
        {
            Producer(connectionString, brokerList, topicName, msg, caCertLocation).Wait();
        }

        public static async Task Producer(string connectionString, string brokerList, string topicName, string msg, string caCertLocation)
        {


            //var config = new ProducerConfig { BootstrapServers = brokerList };

            var config = new ProducerConfig 
            { 
                BootstrapServers = brokerList,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "$ConnectionString",
                SaslPassword = connectionString,
                SslCaLocation = caCertLocation
            };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
      
                try
                {

                    var deliveryReport = await producer.ProduceAsync(
                    topicName, new Message<string, string> { Key = "key" , Value = msg });

                    Console.WriteLine($"delivered to: {deliveryReport.TopicPartitionOffset}");
                    //    }
                }
                catch (ProduceException<string, string> e)
                {
                    Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
                }

            // Since we are producing synchronously, at this point there will be no messages
            // in-flight and no delivery reports waiting to be acknowledged, so there is no
            // need to call producer.Flush before disposing the producer.
        }
        }
    }
}
