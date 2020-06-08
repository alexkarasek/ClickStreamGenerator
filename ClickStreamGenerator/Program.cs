using System;
using Newtonsoft.Json;

namespace ClickStreamGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string brokerList = "[broker address1]:9092,[broker address2]:9092, [broker address3]:9092, [broker address4]:9092";
            string topicName = "test";
            int sessionctr = 0;
            int maxclicks = 1000;
            int maxProductId = 10000;
            int minProductId = 9900;
            int nosessions = 10000;
            int nocats = 10;

            int sessionDuration = 10000;

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
                    KafkaProducer kafkaProducer = new KafkaProducer(brokerList, topicName, click.ToString());
                    
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
