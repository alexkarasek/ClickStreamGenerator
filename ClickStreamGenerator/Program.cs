using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.IO;

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

            string topicName = config["topicName"];
            int sessionctr = Int32.Parse(config["sessionctr"]);
            int maxclicks = Int32.Parse(config["maxclicks"]);
            int maxProductId = Int32.Parse(config["maxProductId"]);
            int minProductId = Int32.Parse(config["minProductId"]);
            int nosessions = Int32.Parse(config["nosessions"]);
            int nocats = Int32.Parse(config["nocats"]);

            string brokerList = config["brokerList"];
            string connectionString = config["connectionString"];
            string caCertLocation =config["caCertLocation"];
            string consumerGroup = config["consumerGroup"];

            
            int sessionDuration = Int32.Parse(config["sessionDuration"]);

            DateTime timestamp = DateTime.Now;
 
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
                    Console.WriteLine(click);
//                    KafkaProducer kafkaProducer = new KafkaProducer(connectionString, brokerList, topicName, click.ToString(), caCertLocation);
                    
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
