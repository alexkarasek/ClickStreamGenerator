using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.IO;
using Confluent.Kafka;

namespace ClickStreamGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot config = builder.Build();

            //string topicName = config["topicName"];

            int sessionctr = Int32.Parse(config["sessionctr"]);
            int maxclicks = Int32.Parse(config["maxclicks"]);
            int maxProductId = Int32.Parse(config["maxProductId"]);
            int minProductId = Int32.Parse(config["minProductId"]);
            int nosessions = Int32.Parse(config["nosessions"]);
            int nocats = Int32.Parse(config["nocats"]);

            Console.WriteLine(args.Length);

            string hostName = null;
            string sasKeyName = null;
            string sasKeyValue = null;
            string eventHubName = null;

            if(args.Length != 4 || args[0]=="" || args[1] =="" || args[2] == "" || args[3] == "")
            {
                hostName = config["hostName"];
                sasKeyName = config["sasKeyName"];
                sasKeyValue = config["sasKeyValue"];
                eventHubName = config["eventHubName"];
            
            }
            else
            {
                hostName = args[0];
                sasKeyName = args[1];
                sasKeyValue = args[2];
                eventHubName = args[3];
                        
            }
            string connectionString = "Endpoint=sb://" + hostName + ".servicebus.windows.net/;SharedAccessKeyName=" + sasKeyName + ";SharedAccessKey=" + sasKeyValue + ";EntityPath=" + eventHubName;

            //string brokerList = config["brokerList"];
            string brokerList = hostName + ".servicebus.windows.net:9093";

            string caCertLocation =config["caCertLocation"];
            string consumerGroup = config["consumerGroup"];

            
            int sessionDuration = Int32.Parse(config["sessionDuration"]);

            DateTime timestamp = DateTime.Now;

            var kconfig = new ProducerConfig 
            { 
                BootstrapServers = brokerList,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "$ConnectionString",
                SaslPassword = connectionString,
                SslCaLocation = caCertLocation
            };
 
            while (++sessionctr < nosessions)
            {
                int clickctr = 0;

                Click click = new Click();
                // Instantiate random number generator using system-supplied value as seed.
                var rand = new Random();

                int noclicks = rand.Next(1, maxclicks);
                DateTime timestamp_new = timestamp.AddSeconds(-1 * sessionDuration);

                //Console.WriteLine("Min timestamp for this session is:" + timestamp_new.ToString());

                while (++clickctr < noclicks)
                {

                    // increment timestamp
                    //timestamp_new = timestamp_new.AddSeconds(rand.Next(0, (int)(timestamp - timestamp_new).TotalSeconds));
                    timestamp_new = timestamp_new.AddSeconds(rand.Next(0, 60));
                    timestamp_new = timestamp_new < timestamp ? timestamp_new : timestamp;
                    //Console.WriteLine("Timestamp is: " + timestamp.ToString() + " : Timestamp_new is: " + timestamp_new.ToString());

                    // seed productid
                    // start with a single product, and decide how many products to iterate through

                    click.sessionid = sessionctr;
                    click.productid = rand.Next(minProductId, maxProductId);
                    click.timestamp = timestamp_new;
                    click.category = click.productid % nocats + 1;

                    //Console.WriteLine("Current Session Id is:" + click.sessionid.ToString());
                    //Console.WriteLine("Current ProductId is:" + click.productid.ToString());
                    //Console.WriteLine("Current Timestamp is:" + click.timestamp.ToString());
                    //Console.WriteLine("Current ProductCategory is:" + click.productid % nocats);
                    Console.WriteLine(clickctr);
                    //Console.WriteLine(connectionString);
                    //Console.WriteLine(topicName);

                    //is this too expensive???
                    //KafkaProducer kafkaProducer = new KafkaProducer(connectionString, brokerList, eventHubName, click.ToString(), caCertLocation);
                    {

                        using (var producer = new ProducerBuilder<string, string>(kconfig).Build())
                        {
                            var deliveryReport =  producer.ProduceAsync(
                            eventHubName, new Message<string, string> { Key = "key" , Value = click.ToString() });

                            //Console.WriteLine($"delivered to: {deliveryReport.()}");
                        }



                    }




                    //Add Category and deserialize class into JSON
                }
            }

            Console.ReadLine();
        }
    }

    public class Click
    {
        public int productid;
        public DateTime timestamp;
        public int sessionid;
        public int category;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


}
